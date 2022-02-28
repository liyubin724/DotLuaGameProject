using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DotEngine.Log
{
    public class LogManager : ILogHandler
    {
        private static LogManager manager = null;
        public static LogManager GetInstance()
        {
            return manager;
        }
        public static LogManager CreateMgr()
        {
            if (manager == null)
            {
                manager = new LogManager();
            }
            return manager;
        }

        public static void DestroyMgr()
        {
            if (manager != null)
            {
                manager.DoDestroy();
            }
            manager = null;
        }

        private Dictionary<string, ILogger> loggerDic = new Dictionary<string, ILogger>();
        private Dictionary<string, ILogAppender> appenderDic = new Dictionary<string, ILogAppender>();

        public LogLevel ValidLogLevel { get; set; } = LogLevelConst.All;
        public LogLevel StacktraceLogLevel { get; set; } = LogLevelConst.Serious;

        private LogManager()
        {
        }

        private void DoDestroy()
        {
            foreach (var kvp in loggerDic)
            {
                kvp.Value.Handler = null;
            }
            foreach (var kvp in appenderDic)
            {
                kvp.Value.DoDestroy();
            }
            loggerDic.Clear();
            appenderDic.Clear();
        }

        public void RemoveLogger(string tag)
        {
            if (loggerDic.TryGetValue(tag, out var logger))
            {
                logger.Handler = null;

                loggerDic.Remove(tag);
            }
        }

        public ILogger GetLogger(string tag, bool createIfNot = true)
        {
            if (!loggerDic.TryGetValue(tag, out var logger) && createIfNot)
            {
                logger = new Logger(tag) { Handler = this };
                loggerDic.Add(tag, logger);
            }
            return logger;
        }

        public void AddAppender(ILogAppender appender)
        {
            string name = appender.Name;
            if (!appenderDic.ContainsKey(name))
            {
                appenderDic.Add(name, appender);
                appender.DoInitialize();
            }
        }

        public void RemoveAppender(string name)
        {
            if (appenderDic.TryGetValue(name, out var appender))
            {
                appenderDic.Remove(name);
                appender.DoDestroy();
            }
        }

        public void OnLogReceived(string tag, LogLevel logLevel, string message)
        {
            if ((ValidLogLevel & logLevel) > 0)
            {
                string stackTrace = string.Empty;
                if ((StacktraceLogLevel & logLevel) > 0)
                {
                    stackTrace = new StackTrace(4, true).ToString();
                }

                foreach (var kvp in appenderDic)
                {
                    ILogAppender appender = kvp.Value;
                    if ((appender.Level & logLevel) > 0)
                    {
                        appender.DoReceive(tag, logLevel, message, stackTrace);
                    }
                }
            }
        }
    }
}
