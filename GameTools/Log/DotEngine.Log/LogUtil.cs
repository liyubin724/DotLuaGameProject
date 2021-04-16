using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotEngine.Log
{
    public static class LogUtil
    {
        private static readonly string DEFAULT_TAG = "Logger";

        public static LogLevel ValidLevel { get; set; } = LogLevelConst.All;
        public static LogLevel StacktraceLevel { get; set; } = LogLevelConst.Serious;

        private static Dictionary<string, Logger> loggerDic = new Dictionary<string, Logger>();
        private static Dictionary<string, ILogAppender> appenderDic = new Dictionary<string, ILogAppender>();

        public static void AddAppender(ILogAppender appender)
        {
            if (!appenderDic.ContainsKey(appender.Name))
            {
                appenderDic.Add(appender.Name, appender);
                appender.DoStart();
            }
        }

        public static void RemoveAppender(string name)
        {
            if (appenderDic.TryGetValue(name, out var appender))
            {
                appenderDic.Remove(name);
                appender.DoEnd();
            }
        }

        public static Logger GetLogger(string tag, LogLevel logLevel)
        {
            if (!loggerDic.TryGetValue(tag, out var logger))
            {
                logger = new Logger(tag)
                {
                    Handler = OnLogMessage,
                };
                loggerDic.Add(tag, logger);
            }
            logger.ValidLevel = logLevel;

            return logger;
        }

        public static Logger GetLogger(string tag)
        {
            return GetLogger(tag, ValidLevel);
        }

        public static void RemoveLogger(string tag)
        {
            if (loggerDic.ContainsKey(tag))
            {
                loggerDic.Remove(tag);
            }
        }

        private static void OnLogMessage(LogLevel level, string tag, string message)
        {
            if (level <= ValidLevel)
            {
                return;
            }

            foreach (var kvp in appenderDic)
            {
                if ((kvp.Value.ValidLevel & level) > 0)
                {
                    kvp.Value.OnLogReceived(level, tag, DateTime.Now, message, GetStackTrace(level));
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
            OnLogMessage(LogLevel.Trace, DEFAULT_TAG, message);
        }

        public static void Trace(string tag, string message)
        {
            GetLogger(tag)?.Trace(message);
        }

        public static void Debug(string message)
        {
            OnLogMessage(LogLevel.Debug, DEFAULT_TAG, message);
        }

        public static void Debug(string tag, string message)
        {
            GetLogger(tag)?.Debug(message);
        }

        public static void Info(string message)
        {
            OnLogMessage(LogLevel.Info, DEFAULT_TAG, message);
        }

        public static void Info(string tag, string message)
        {
            GetLogger(tag)?.Info(message);
        }

        public static void Warning(string message)
        {
            OnLogMessage(LogLevel.Warning, DEFAULT_TAG, message);
        }

        public static void Warning(string tag, string message)
        {
            GetLogger(tag)?.Warning(message);
        }

        public static void Error(string message)
        {
            OnLogMessage(LogLevel.Error, DEFAULT_TAG, message);
        }

        public static void Error(string tag, string message)
        {
            GetLogger(tag)?.Error(message);
        }

        public static void Fatal(string message)
        {
            OnLogMessage(LogLevel.Fatal, DEFAULT_TAG, message);
        }

        public static void Fatal(string tag, string message)
        {
            GetLogger(tag)?.Fatal(message);
        }

        private static string GetStackTrace(LogLevel level)
        {
            if ((StacktraceLevel & level) > 0)
            {
                return string.Empty;
            }
            return new StackTrace(3, true).ToString();
        }
    }
}
