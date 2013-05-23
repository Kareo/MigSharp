using System;

namespace MigSharp.Core
{
    internal class MigrationMetadata : IMigrationMetadata
    {
        private readonly long _timestamp;
        private readonly string _moduleName;
        private readonly string _tag;

        /// <summary>
        /// Gets the timestamp of the migration.
        /// </summary>
        public long Timestamp { get { return _timestamp; } }

        /// <summary>
        /// The fully qualified class name of the migration
        /// </summary>
        public string MigrationName { get; private set; }

        /// <summary>
        /// Date Timestamp was applied
        /// </summary>
        public DateTime? AppliedDate { get; private set; }

        /// <summary>
        /// Gets the module name of the migration.
        /// </summary>
        public string ModuleName { get { return _moduleName; } }

        /// <summary>
        /// Gets the associated tag of the migration.
        /// </summary>
        public string Tag { get { return _tag; } }

        public MigrationMetadata(long timestamp, string moduleName, string tag, string migrationName = null, DateTime? appliedDate = null)
        {
            MigrationName = migrationName;
            AppliedDate = appliedDate;
            _timestamp = timestamp;
            _moduleName = moduleName;
            _tag = tag;
        }
    }
}