using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MigSharp.Core;
using MigSharp.Core.Entities;
using MigSharp.Providers;

namespace MigSharp.Process
{
    internal class BootstrapMigrationStep
    {
        private readonly IMigration _migration;
        private readonly IProvider _provider;
        private readonly IProviderMetadata _providerMetadata;
        private static readonly Regex goStatementRegex = new Regex(@"^\s*GO\s*$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        protected IMigration Migration { get { return _migration; } }
        protected IProviderMetadata ProviderMetadata { get { return _providerMetadata; } }

        public BootstrapMigrationStep(IMigration migration, IProvider provider, IProviderMetadata providerMetadata)
        {
            _migration = migration;
            _provider = provider;
            _providerMetadata = providerMetadata;
        }

        internal void Execute(IDbConnection connection, IDbTransaction transaction, MigrationDirection direction, IDbCommandExecutor commandExecutor)
        {
            Debug.Assert(connection.State == ConnectionState.Open);

            var context = new RuntimeContext(connection, transaction, commandExecutor, _providerMetadata);
            Database database = GetDatabaseContainingMigrationChanges(direction, context);
            var translator = new CommandsToSqlTranslator(_provider);
            foreach (string commandText in translator.TranslateToSql(database, context))
            {
                //MigSharp uses ADO, not SMO to run SQL. ADO does not support the 'GO' statement in SQL as it is not TSQL.
                //We split SQL on 'GO' and run as individual statements
                var separatedByGoStatements = new List<string>(goStatementRegex.Split(commandText));
                separatedByGoStatements.RemoveAll(r => String.IsNullOrEmpty(r.Trim()));

                foreach (var statement in separatedByGoStatements)
                {
                    IDbCommand command = connection.CreateCommand();
                    command.CommandTimeout = 0; // do not timeout; the client is responsible for not causing lock-outs
                    command.Transaction = transaction;
                    command.CommandText = statement;
                    try
                    {
                        commandExecutor.ExecuteNonQuery(command);
                    }
                    catch (DbException x)
                    {
                        Log.Error("An error occurred: {0}{1}while trying to execute:{1}{2}", x.Message, Environment.NewLine, command.CommandText);
                        throw;
                    }
                }
            }
        }

        protected Database GetDatabaseContainingMigrationChanges(MigrationDirection direction, IMigrationContext context)
        {
            var database = new Database(context);
            if (direction == MigrationDirection.Up)
            {
                _migration.Up(database);
            }
            else
            {
                Debug.Assert(direction == MigrationDirection.Down);
                var migration = _migration as IReversibleMigration;
                if (migration == null)
                {
                    throw new InvalidOperationException("Cannot downgrade an irreversible migration."); // this should never happen
                }
                migration.Down(database);
            }
            return database;
        }
    }
}