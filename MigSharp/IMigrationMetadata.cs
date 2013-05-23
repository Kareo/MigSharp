using System;
using MigSharp.Process;

namespace MigSharp
{
    /// <summary>
    /// Represents all descriptive information about a migration.
    /// </summary>
    public interface IMigrationMetadata : IMigrationExportMetadata
    {
        /// <summary>
        /// Gets the timestamp of the migration.
        /// </summary>
        long Timestamp { get; }

        /// <summary>
        /// 
        /// </summary>
        string MigrationName { get;}

        /// <summary>
        /// 
        /// </summary>
        DateTime? AppliedDate { get;}
    }
}