﻿using System.Diagnostics;

namespace DotEngine.Log
{
    public delegate void LogHandler(LogLevel level, string tag, string message, string stackTrace);

    public class Logger
    {
        public string Tag { get; private set; }
        public LogLevel ValidLevel { get; set; } = LogLevelConst.All;
        public LogLevel StacktraceLevel { get; set; } = LogLevelConst.Serious;

        private LogHandler logHandler = null;

        internal Logger(string tag, LogHandler handler)
        {
            Tag = tag;
            logHandler = handler;
        }

        private string GetStackTrace(LogLevel level)
        {
            if ((StacktraceLevel & level) > 0)
            {
                return new StackTrace(3, true).ToString();
            }
            return string.Empty;
        }

        public void Trace(string message)
        {
            if ((ValidLevel & LogLevel.Trace) > 0)
            {
                logHandler?.Invoke(LogLevel.Trace, Tag, message, GetStackTrace(LogLevel.Trace));
            }
        }

        public void Debug(string message)
        {
            if ((ValidLevel & LogLevel.Debug) > 0)
            {
                logHandler?.Invoke(LogLevel.Debug, Tag, message, GetStackTrace(LogLevel.Debug));
            }
        }

        public void Info(string message)
        {
            if ((ValidLevel & LogLevel.Info) > 0)
            {
                logHandler?.Invoke(LogLevel.Info, Tag, message, GetStackTrace(LogLevel.Info));
            }
        }

        public void Warning(string message)
        {
            if ((ValidLevel & LogLevel.Warning) > 0)
            {
                logHandler?.Invoke(LogLevel.Warning, Tag, message, GetStackTrace(LogLevel.Warning));
            }
        }

        public void Error(string message)
        {
            if ((ValidLevel & LogLevel.Error) > 0)
            {
                logHandler?.Invoke(LogLevel.Error, Tag, message, GetStackTrace(LogLevel.Error));
            }
        }

        public void Fatal(string message)
        {
            if ((ValidLevel & LogLevel.Fatal) > 0)
            {
                logHandler?.Invoke(LogLevel.Fatal, Tag, message, GetStackTrace(LogLevel.Fatal));
            }
        }
    }
}
