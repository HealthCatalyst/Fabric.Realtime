namespace Catalyst.Logging.Abstractions
{
    using System;
    using System.ComponentModel;

    public static class LoggingServiceExtensions
    {
        public static void Verbose(this ILoggingService logger, [Localizable(false)] string message)
        {
            ArgumentNotNull(logger);
            logger.Log(new LogEntry(LoggingEventType.Verbose, message));
        }

        public static void Debug(this ILoggingService logger, [Localizable(false)] string message)
        {
            ArgumentNotNull(logger);
            logger.Log(new LogEntry(LoggingEventType.Debug, message));
        }

        public static void Info(this ILoggingService logger, [Localizable(false)] string message)
        {
            ArgumentNotNull(logger);
            logger.Log(new LogEntry(LoggingEventType.Information, message));
        }

        public static void Warn(this ILoggingService logger, [Localizable(false)] string message, Exception exception = null)
        {
            ArgumentNotNull(logger);
            logger.Log(new LogEntry(LoggingEventType.Warning, message, exception));
        }

        public static void Error(this ILoggingService logger, [Localizable(false)] string message, Exception exception = null)
        {
            ArgumentNotNull(logger);
            logger.Log(new LogEntry(LoggingEventType.Error, message, exception));
        }

        public static void Fatal(this ILoggingService logger, [Localizable(false)] string message, Exception exception = null)
        {
            ArgumentNotNull(logger);
            logger.Log(new LogEntry(LoggingEventType.Fatal, message, exception));
        }

        private static void ArgumentNotNull(ILoggingService logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
        }
    }
}