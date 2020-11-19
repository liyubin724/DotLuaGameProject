using System.Diagnostics;

namespace DotEngine.Log
{
    public delegate void LogMessage(Logger logger, LogLevel level, string message, string stackTrace);

    public class Logger
    {
        public string Name { get; private set; }
        public LogLevel MinLogLevel { get; set; } = LogLevel.On;
        public LogLevel StackTraceLogLevel { get; set; } = LogLevel.Error;
        public LogMessage OnLogMessage { get; set; }

        internal Logger(string name)
        {
            Name = name;
        }

        private string GetStackTrace(LogLevel level)
        {
            if(level< StackTraceLogLevel)
            {
                return string.Empty;
            }
            return new StackTrace(3,true).ToString();
        }

        public void Dispose()
        {
            OnLogMessage = null;
            LogUtil.RemoveLogger(Name);
        }

        public void Trace(string message)
        {
            if(MinLogLevel<LogLevel.Trace)
            {
                OnLogMessage?.Invoke(this, LogLevel.Trace, message, GetStackTrace(LogLevel.Trace));
            }
        }

        public void Debug(string message)
        {
            if (MinLogLevel < LogLevel.Debug)
            {
                OnLogMessage?.Invoke(this, LogLevel.Debug, message, GetStackTrace(LogLevel.Debug));
            }
        }

        public void Info(string message)
        {
            if (MinLogLevel < LogLevel.Info)
            {
                OnLogMessage?.Invoke(this, LogLevel.Info, message, GetStackTrace(LogLevel.Info));
            }
        }

        public void Warning(string message)
        {
            if (MinLogLevel < LogLevel.Warning)
            {
                OnLogMessage?.Invoke(this, LogLevel.Warning, message, GetStackTrace(LogLevel.Warning));
            }
        }

        public void Error(string message)
        {
            if (MinLogLevel < LogLevel.Error)
            {
                OnLogMessage?.Invoke(this, LogLevel.Error, message, GetStackTrace(LogLevel.Error));
            }
        }

        public void Fatal(string message)
        {
            if (MinLogLevel < LogLevel.Fatal)
            {
                OnLogMessage?.Invoke(this, LogLevel.Fatal, message, GetStackTrace(LogLevel.Fatal));
            }
        }
    }
}
