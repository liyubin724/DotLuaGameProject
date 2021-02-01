using DotEngine.Log.Appender;
using System.Collections.Generic;

namespace DotEngine.Log
{
    public static class LogUtil
    {
        public static LogLevel GlobalLogLevel { get; set; } = LogLevel.On;

        internal static Dictionary<string, Logger> Loggers { get; private set; } = new Dictionary<string, Logger>();
        private readonly static Dictionary<string, ALogAppender> sm_Appenders = new Dictionary<string, ALogAppender>();

        private static Logger sm_DefaultLogger = null;
        static LogUtil()
        {
            sm_DefaultLogger = GetLogger("Logger", LogLevel.On, LogLevel.Error);

            //DebugLog.SetLogAction(sm_DefaultLogger.Info, sm_DefaultLogger.Warning, sm_DefaultLogger.Error);
        }

        public static void AddAppender(ALogAppender appender)
        {
            if(!sm_Appenders.ContainsKey(appender.Name))
            {
                sm_Appenders.Add(appender.Name, appender);
            }
        }

        public static void RemoveAppender(string name)
        {
            if(sm_Appenders.TryGetValue(name,out var appender))
            {
                appender.Dispose();

                sm_Appenders.Remove(name);
            }
        }

        public static Logger GetLogger(string name,LogLevel logLevel = LogLevel.Trace,LogLevel stackTraceLevel = LogLevel.Error)
        {
            if(!Loggers.TryGetValue(name,out var logger))
            {
                logger = new Logger(name)
                {
                    OnLogMessage = OnLogMessage,
                    MinLogLevel = logLevel,
                    StackTraceLogLevel = stackTraceLevel,
                };
                Loggers.Add(name, logger);
            }
            return logger;
        }

        public static void RemoveLogger(string name)
        {
            if(Loggers.ContainsKey(name))
            {
                Loggers.Remove(name);
            }
        }

        private static void OnLogMessage(Logger logger, LogLevel level, string message,string stackTrace)
        {
            if (level < GlobalLogLevel)
            {
                return;
            }

            foreach (var kvp in sm_Appenders)
            {
                kvp.Value.OnLogReceived(level, logger.Name, message,stackTrace);
            }
        }

        public static void Reset()
        {
            foreach (var kvp in sm_Appenders)
            {
                kvp.Value.Dispose();
            }
            sm_Appenders.Clear();
            Loggers.Clear();
        }

        public static void Trace(string message)
        {
            sm_DefaultLogger.Trace(message);
        }

        public static void Trace(string tag,string message)
        {
            GetLogger(tag)?.Trace(message);
        }

        public static void Debug(string message)
        {
            sm_DefaultLogger.Debug(message);
        }

        public static void Debug(string tag,string message)
        {
            GetLogger(tag)?.Debug(message);
        }

        public static void Info(string message)
        {
            sm_DefaultLogger.Info(message);
        }

        public static void Info(string tag,string message)
        {
            GetLogger(tag)?.Info(message);
        }

        public static void Warning(string message)
        {
            sm_DefaultLogger.Warning(message);
        }

        public static void Warning(string tag,string message)
        {
            GetLogger(tag)?.Warning(message);
        }

        public static void Error(string message)
        {
            sm_DefaultLogger.Error(message);
        }

        public static void Error(string tag,string message)
        {
            GetLogger(tag)?.Error(message);
        }

        public static void Fatal(string message)
        {
            sm_DefaultLogger.Fatal(message);
        }

        public static void Fatal(string tag,string message)
        {
            GetLogger(tag)?.Fatal(message);
        }
    }
}
