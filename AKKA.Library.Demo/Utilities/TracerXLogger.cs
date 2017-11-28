using Akka.Actor;
using Akka.Dispatch;
using Akka.Event;
using System;
using TracerX;
using AkkaLogLevel = TracerX.TraceLevel;

namespace AKKA.Library.Demo
{
    public class TracerXLogger : ReceiveActor,
                                 IRequiresMessageQueue<ILoggerMessageQueueSemantics>,
                                ILogReceive
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        private static Logger _logger;

        private static Logger GetLogger(LogEvent logEvent)
        {
            if (_logger == null)
            {
                _logger = Logger.GetLogger(logEvent.LogClass.FullName);
                if (!Logger.DefaultBinaryFile.IsOpen)
                    Logger.DefaultBinaryFile.Open();
                if (!Logger.DefaultTextFile.IsOpen)
                    Logger.DefaultTextFile.Open();
                Logger.Root.BinaryFileTraceLevel = TraceLevel.Verbose;
            }
            return _logger;
        }

        private static void Log(LogEvent logEvent, Action<Logger, string> logStatement)
        {
            var logger = GetLogger(logEvent);
            logStatement(logger, logEvent.LogSource);
        }

        public TracerXLogger()
        {
            Receive<Error>(m => Log(m, (logger, logSource) => LogEvent(logger, AkkaLogLevel.Error, logSource, m.Cause, "{0}", m.Message)));
            Receive<Warning>(m => Log(m, (logger, logSource) => LogEvent(logger, AkkaLogLevel.Warn, logSource, "{0}", m.Message)));
            Receive<Info>(m => Log(m, (logger, logSource) => LogEvent(logger, AkkaLogLevel.Info, logSource, "{0}", m.Message)));
            Receive<Debug>(m => Log(m, (logger, logSource) => LogEvent(logger, AkkaLogLevel.Debug, logSource, "{0}", m.Message)));
            Receive<InitializeLogger>(m =>
            {
                _log.Info("TracerXLogger started");
                Sender.Tell(new LoggerInitialized());
            });
        }

        private static void LogEvent(Logger logger, AkkaLogLevel level, string logSource, string message, params object[] parameters)
        {
            LogEvent(logger, level, logSource, null, message, parameters);
        }

        private static void LogEvent(Logger logger, AkkaLogLevel level, string logSource, Exception exception, string message, params object[] parameters)
        {
            Write(logger, level, parameters);
            //var logEvent = new LogEventInfo(level, logger.Name, null, message, parameters, exception);
            //logEvent.Properties["logSource"] = logSource;
            //logger.Log(logEvent);
        }

        public static void Write(Logger logger, AkkaLogLevel level, params object[] parms)
        {
            switch (level)
            {
                case AkkaLogLevel.Fatal:
                    logger.Fatal(parms);
                    break;

                case AkkaLogLevel.Error:
                    logger.Error(parms);
                    break;

                case AkkaLogLevel.Warn:
                    logger.Warn(parms);
                    break;

                case AkkaLogLevel.Info:
                    logger.Info(parms);
                    break;

                case AkkaLogLevel.Debug:
                    logger.Debug(parms);
                    break;

                case AkkaLogLevel.Verbose:
                    logger.Verbose(parms);
                    break;

                default:
                    break;
            }
        }
    }
}