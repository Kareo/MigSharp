using System;

namespace MigSharp
{
    public class MigrationExecutedEventArgs : MigrationEventArgs
    {
        public MigrationExecutedMetadata ExecutedMetadata { get; set; }

        public MigrationExecutedEventArgs(IScheduledMigrationMetadata metadata, MigrationExecutedMetadata executedMetadata) : base(metadata)
        {
            ExecutedMetadata = executedMetadata;
        }
    }

    public class MigrationExecutedMetadata
    {
        public MigrationExecutedMetadata(TimeSpan executionTime)
        {
            ExecutionTime = executionTime;
        }

        /// <summary>
        /// Execution Time of the given Migration
        /// </summary>
        public TimeSpan ExecutionTime { get; private set; }
    }
}