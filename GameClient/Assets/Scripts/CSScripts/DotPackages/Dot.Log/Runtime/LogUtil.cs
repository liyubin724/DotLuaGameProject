using System;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Log
{
    public static class LogUtil
    {
        public static LogLevel GlobalValidLevel { get; set; } = LogLevelConst.All;
        public static LogLevel GlobalStacktraceLevel { get; set; } = LogLevelConst.Serious;

        private static Dictionary<string, Logger> loggerDic = new Dictionary<string, Logger>();
        private readonly static Dictionary<string, ILogAppender> appenderDic = new Dictionary<string, ILogAppender>();

        private static Logger defaultLogger = null;
        static LogUtil()
        {
            defaultLogger = GetLogger("Logger", LogLevel.Off, LogLevel.Error);
        }

        public static void AddAppender(ILogAppender appender)
        {
            if(!appenderDic.ContainsKey(appender.Name))
            {
                appenderDic.Add(appender.Name, appender);
                appender.DoStart();
            }
        }

        public static void RemoveAppender(string name)
        {
            if(appenderDic.TryGetValue(name,out var appender))
            {
                appenderDic.Remove(name);
                appender.DoEnd();
            }
        }

        public static Logger GetLogger(string tag)
        {
            return GetLogger(tag, GlobalValidLevel, GlobalStacktraceLevel);
        }

        public static Logger GetLogger(string tag, LogLevel logLevel, LogLevel stackTraceLevel)
        {
            if (!loggerDic.TryGetValue(tag, out var logger))
            {
                logger = new Logger(tag)
                {
                    ValidLevel = logLevel,
                    StacktraceLevel = stackTraceLevel,
                    Handler = OnLogMessage,
                };

                loggerDic.Add(tag, logger);
            }
            return logger;
        }

        public static void RemoveLogger(string tag)
        {
            if(loggerDic.ContainsKey(tag))
            {
                loggerDic.Remove(tag);
            }
        }

        private static void OnLogMessage(LogLevel level, string tag, string message,string stackTrace)
        {
            if((GlobalValidLevel & level) > 0)
            {
                foreach (var kvp in appenderDic)
                {
                    kvp.Value.OnLogReceived(level, DateTime.Now, tag, message,stackTrace);
                }
            }
        }

        public static void Reset()
        {
            var keys = loggerDic.Keys.ToArray();
            foreach (var key in keys)
            {
                RemoveLogger(key);
            }

            keys = appenderDic.Keys.ToArray();
            foreach (var key in keys)
            {
                RemoveAppender(key);
            }
        }

        public static void Trace(string message)
        {
            defaultLogger.Trace(message);
        }

        public static void Trace(string tag,string message)
        {
            GetLogger(tag)?.Trace(message);
        }

        public static void Debug(string message)
        {
            defaultLogger.Debug(message);
        }

        public static void Debug(string tag,string message)
        {
            GetLogger(tag)?.Debug(message);
        }

        public static void Info(string message)
        {
            defaultLogger.Info(message);
        }

        public static void Info(string tag,string message)
        {
            GetLogger(tag)?.Info(message);
        }

        public static void Warning(string message)
        {
            defaultLogger.Warning(message);
        }

        public static void Warning(string tag,string message)
        {
            GetLogger(tag)?.Warning(message);
        }

        public static void Error(string message)
        {
            defaultLogger.Error(message);
        }

        public static void Error(string tag,string message)
        {
            GetLogger(tag)?.Error(message);
        }

        public static void Fatal(string message)
        {
            defaultLogger.Fatal(message);
        }

        public static void Fatal(string tag,string message)
        {
            GetLogger(tag)?.Fatal(message);
        }
    }
}
