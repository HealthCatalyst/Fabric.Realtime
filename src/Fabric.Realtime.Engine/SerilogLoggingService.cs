using System;
using Catalyst.Logging.Abstractions;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Fabric.Realtime.Engine
{
    /// <summary>
    /// Logging service implementation that wraps Serilog.
    /// </summary>
    public class SerilogLoggingService : ILoggingService
    {
        private readonly Logger logger;

        /// <summary>
        /// Create a new SerilogLoggingService.
        /// </summary>
        public SerilogLoggingService()
        {
            logger = new LoggerConfiguration()
                .WriteTo
                .Console()
                .CreateLogger();
        }

        public void Log(LogEntry entry)
        {
            var level = TranslateLogLevel(entry);
            logger.Write(level, entry.Exception, entry.Message);
        }

        private static LogEventLevel TranslateLogLevel(LogEntry entry)
        {
            switch (entry.Severity)
            {
                case LoggingEventType.Verbose:
                    return LogEventLevel.Verbose;
                case LoggingEventType.Debug:
                    return LogEventLevel.Debug;
                case LoggingEventType.Information:
                    return LogEventLevel.Information;
                case LoggingEventType.Warning:
                    return LogEventLevel.Warning;
                case LoggingEventType.Error:
                    return LogEventLevel.Error;
                case LoggingEventType.Fatal:
                    return LogEventLevel.Fatal;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}