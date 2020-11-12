namespace DotEngine.Log
{
    public delegate void LogMessage(Logger logger, LogLevel level, string message);

    public class Logger
    {
        public string Name { get; private set; }
        public LogLevel MinLogLevel { get; private set; }

        private LogMessage m_OnLog;

        internal Logger(string name, LogLevel minLevel, LogMessage onLogMessage)
        {
            Name = name;
            MinLogLevel = minLevel;
            m_OnLog = onLogMessage;
        }

        public void Trace(string message)
        {
            if(MinLogLevel<LogLevel.Trace)
            {
                m_OnLog?.Invoke(this, LogLevel.Trace, message);
            }
        }

        public void Debug(string message)
        {
            if (MinLogLevel < LogLevel.Debug)
            {
                m_OnLog?.Invoke(this, LogLevel.Debug, message);
            }
        }

        public void Info(string message)
        {
            if (MinLogLevel < LogLevel.Info)
            {
                m_OnLog?.Invoke(this, LogLevel.Info, message);
            }
        }

        public void Warning(string message)
        {
            if (MinLogLevel < LogLevel.Warning)
            {
                m_OnLog?.Invoke(this, LogLevel.Warning, message);
            }
        }

        public void Error(string message)
        {
            if (MinLogLevel < LogLevel.Error)
            {
                m_OnLog?.Invoke(this, LogLevel.Error, message);
            }
        }

        public void Fatal(string message)
        {
            if (MinLogLevel < LogLevel.Fatal)
            {
                m_OnLog?.Invoke(this, LogLevel.Fatal, message);
            }
        }
    }
}
