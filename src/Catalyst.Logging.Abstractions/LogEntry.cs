namespace Catalyst.Logging.Abstractions
{
    using System;

    public enum LoggingEventType
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }

    public class LogEntry
    {
        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            this.Severity = severity;
            this.Message = message;
            this.Exception = exception;
        }

        public LoggingEventType Severity { get; }

        public string Message { get; }

        public Exception Exception { get; }
    }
}