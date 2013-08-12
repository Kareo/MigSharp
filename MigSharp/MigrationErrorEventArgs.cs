using System;

namespace MigSharp
{
    public class MigrationErrorEventArgs : EventArgs
    {
        public IScheduledMigrationMetadata Metadata { get; private set; }
        public Exception Exception { get; private set; }

        public MigrationErrorEventArgs(IScheduledMigrationMetadata metadata, Exception exception)
        {
            Metadata = metadata;
            Exception = exception;
        }
    }
}