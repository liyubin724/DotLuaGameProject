using System;

namespace DotEngine.Log
{
    public abstract class ALogAppender : ILogAppender
    {
        public string Name { get; private set; }
        public LogLevel ValidLevel { get; set; } = LogLevelConst.All;

        protected ILogFormatter Formatter { get; set; }

        public ALogAppender(string name) : this(name, new DefaultLogFormatter())
        { }

        public ALogAppender(string name, ILogFormatter formatter)
        {
            Name = name;
            Formatter = formatter;
        }

        public virtual void DoInitialize()
        {
        }

        public void OnLogReceived(string tag, LogLevel level, DateTime dateTime, string message, string stacktrace)
        {
            if ((ValidLevel & level) > 0)
            {
                string logMessage = Formatter.FormatMessage(tag, level, dateTime, message, stacktrace);
                LogMessage(level, logMessage);
            }
        }

        protected abstract void LogMessage(LogLevel level, string message);

        public virtual void DoDestroy()
        {
        }
    }
}
