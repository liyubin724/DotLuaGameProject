using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DotEngine.Log
{
    public class LogManager : ILogHandler
    {
        public static string DEFAULT_LOGGER_TAG = "Logger";

        private static LogManager manager = null;
        public static LogManager GetInstance()
        {
            return manager;
        }
        public static LogManager InitMgr()
        {
            if(manager == null)
            {
                manager = new LogManager();
                manager.DoInitialize();
            }
            return manager;
        }

        public static void DisposeMgr()
        {
            if(manager!=null)
            {
                manager.DoDestroy();
            }
            manager = null;
        }

        private Dictionary<string, ILogger> loggerDic = new Dictionary<string, ILogger>();
        private Dictionary<string, ILogAppender> appenderDic = new Dictionary<string, ILogAppender>();

        public LogLevel ValidLogLevel { get; set; } = LogLevel.All;
        public LogLevel StacktraceLogLevel { get; set; } = LogLevelConst.Serious;

        private LogManager()
        {
        }

        private void DoInitialize()
        {
            CreateLogger(DEFAULT_LOGGER_TAG);
        }

        private void DoDestroy()
        {
            foreach(var kvp in loggerDic)
            {
                kvp.Value.Handler = null;
            }
            foreach(var kvp in appenderDic)
            {
                kvp.Value.DoDestroy();
            }
            loggerDic.Clear();
            appenderDic.Clear();
        }

        public void AddLogger(ILogger logger)
        {
            string tag = logger.Tag;
            if(!loggerDic.ContainsKey(tag))
            {
                logger.Handler = this;
                loggerDic.Add(tag, logger);
            }
        }

        public void RemoveLogger(string tag)
        {
            ILogger logger = loggerDic[tag];
            if(logger!=null)
            {
                logger.Handler = null;
                loggerDic.Remove(tag);
            }
        }

        public ILogger CreateLogger(string tag)
        {
            ILogger logger = new Logger(tag)
            {
                Handler = this
            };

            loggerDic.Add(tag, logger);

            return logger;
        }

        public ILogger GetLogger(string tag,bool createIfNot = true)
        {
            if(!loggerDic.TryGetValue(tag,out var logger) && createIfNot)
            {
                logger = CreateLogger(tag);
            }
            return logger;
        }

        public void AddAppender(ILogAppender appender)
        {
            string name = appender.Name;
            if(!appenderDic.ContainsKey(name))
            {
                appenderDic.Add(name, appender);
                appender.DoInitialize();
            }
        }

        public void RemoveAppender(string name)
        {
            if(appenderDic.TryGetValue(name,out var appender))
            {
                appenderDic.Remove(name);
                appender.DoDestroy();
            }
        }

        public void OnLogReceived(string tag, LogLevel logLevel, string message)
        {
            if((ValidLogLevel & logLevel)>0)
            {
                DateTime time = DateTime.Now;
                string stacktrace = null;
                if((StacktraceLogLevel & logLevel) > 0)
                {
                    stacktrace = new StackTrace(4, true).ToString();
                }

                foreach(var kvp in appenderDic)
                {
                    ILogAppender appender = kvp.Value;
                    if((appender.ValidLevel & logLevel) > 0)
                    {
                        appender.OnLogReceived(tag, logLevel, time, message, stacktrace);
                    }
                }
            }
        }
    }
}
