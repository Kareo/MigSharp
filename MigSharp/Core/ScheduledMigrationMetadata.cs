using System;

namespace MigSharp.Core
{
    internal class ScheduledMigrationMetadata : MigrationMetadata, IScheduledMigrationMetadata
    {
        private readonly MigrationDirection _direction;

        public MigrationDirection Direction { get { return _direction; } }

        public ScheduledMigrationMetadata(long timestamp, string moduleName, string tag, string migrationName, DateTime? appliedDate, MigrationDirection direction) 
            : base(timestamp, moduleName, tag, migrationName, appliedDate)
        {
            _direction = direction;
        }
    }
}