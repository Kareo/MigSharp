using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

using MigSharp.Core;
using MigSharp.Process;
using MigSharp.Providers;

namespace MigSharp
{
    public class Migrator
    {
        private readonly ConnectionInfo _connectionInfo;

        /// <summary>
        /// Initializes a new instance of <see cref="Migrator"/>.
        /// </summary>
        /// <param name="connectionString">Connection string to the database to be migrated.</param>
        /// <param name="providerInvariantName">Invariant name of a provider. <seealso cref="DbProviderFactories.GetFactory(string)"/></param>
        public Migrator(string connectionString, string providerInvariantName)
        {
            _connectionInfo = new ConnectionInfo(connectionString, providerInvariantName);
        }

        public void UpgradeAll(Assembly assembly)
        {
            Log.Info("Upgrading all...");
            Updgrade(assembly, DateTime.MaxValue);
        }

        public void UpgradeUntil(Assembly assembly, DateTime timestamp)
        {
            Log.Info(string.Format(CultureInfo.CurrentCulture, "Upgrading until {0}...", timestamp));
            Updgrade(assembly, timestamp);
        }

        private void Updgrade(Assembly assembly, DateTime timestamp)
        {
            DateTime start = DateTime.Now;
            List<Lazy<IMigration, IMigrationMetadata>> migrations = CollectMigrations(assembly);
            Log.Info(LogCategory.Performance, "Collecting migrations took {0}ms", (DateTime.Now - start).TotalMilliseconds);

            if (migrations.Count > 0)
            {
                var providerFactory = new ProviderFactory();
                var connectionFactory = new DbConnectionFactory();
                var dbVersion = DbVersion.Create(_connectionInfo, providerFactory, connectionFactory);
                var applicableMigrations = from m in migrations
                                           where m.Metadata.Timestamp() <= timestamp && !dbVersion.Includes(m.Metadata)
                                           orderby m.Metadata.Timestamp()
                                           select m;
                int count = applicableMigrations.Count();
                Log.Info("Found {0} applicable migration(s)", count);
                if (count > 0)
                {
                    var batch = new MigrationBatch(applicableMigrations, _connectionInfo, providerFactory, connectionFactory);
                    batch.Execute(dbVersion);
                }
            }
            Log.Info(LogCategory.Performance, "All migration(s) took {0}s", (DateTime.Now - start).TotalSeconds);
        }

        private static List<Lazy<IMigration, IMigrationMetadata>> CollectMigrations(Assembly assembly)
        {
            Log.Info("Collecting all migrations...");
            var catalog = new AssemblyCatalog(assembly);
            var container = new CompositionContainer(catalog);
            var migrationImporter = new MigrationImporter();
            container.ComposeParts(migrationImporter);
            var result = new List<Lazy<IMigration, IMigrationMetadata>>(migrationImporter.Migrations);
            Log.Info("Found {0} migration(s) in total", result.Count);
            return result;
        }

        private class MigrationImporter
        {
// ReSharper disable UnusedAutoPropertyAccessor.Local
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            [ImportMany]
            public IEnumerable<Lazy<IMigration, IMigrationMetadata>> Migrations { get; set; } // set by MEF
// ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}