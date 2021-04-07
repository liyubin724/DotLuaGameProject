using System.Diagnostics;

namespace DotEngine.Log
{
    public delegate void LogHandler(LogLevel level, string tag, string message, string stackTrace);

    public class Logger
    {
        public string Tag { get; private set; }
        public LogLevel MinLogLevel { get; set; } = LogLevel.On;
        public LogLevel StackTraceLogLevel { get; set; } = LogLevel.Error;

        private LogHandler Handler;

        internal Logger(string tag,LogHandler handler)
        {
            Tag = tag;
            Handler = handler;
        }

        private string GetStackTrace(LogLevel level)
        {
            if(level < StackTraceLogLevel)
            {
                return string.Empty;
            }
            return new StackTrace(3, true).ToString();
        }

        public void Trace(string message)
        {
            if (MinLogLevel < LogLevel.Trace)
            {
                Handler?.Invoke(LogLevel.Trace,Tag, message, GetStackTrace(LogLevel.Trace));
            }
        }

        public void Debug(string message)
        {
            if (MinLogLevel < LogLevel.Debug)
            {
                Handler?.Invoke(LogLevel.Debug, Tag, message, GetStackTrace(LogLevel.Debug));
            }
        }

        public void Info(string message)
        {
            if (MinLogLevel < LogLevel.Info)
            {
                Handler?.Invoke(LogLevel.Info, Tag, message, GetStackTrace(LogLevel.Info));
            }
        }

        public void Warning(string message)
        {
            if (MinLogLevel < LogLevel.Warning)
            {
                Handler?.Invoke(LogLevel.Warning,Tag, message, GetStackTrace(LogLevel.Warning));
            }
        }

        public void Error(string message)
        {
            if (MinLogLevel < LogLevel.Error)
            {
                Handler?.Invoke(LogLevel.Error, Tag, message, GetStackTrace(LogLevel.Error));
            }
        }

        public void Fatal(string message)
        {
            if (MinLogLevel < LogLevel.Fatal)
            {
                Handler?.Invoke(LogLevel.Fatal, Tag, message, GetStackTrace(LogLevel.Fatal));
            }
        }
    }
}
