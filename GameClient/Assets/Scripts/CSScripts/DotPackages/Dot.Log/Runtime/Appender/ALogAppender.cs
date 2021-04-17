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

        public virtual void DoStart()
        {
        }

        public void OnLogReceived(LogLevel level, DateTime dateTime, string tag, string message, string stacktrace)
        {
            if ((ValidLevel & level) > 0)
            {
                string logMessage = Formatter.FormatMessage(level, dateTime, tag, message, stacktrace);
                OutputLogMessage(level, logMessage);
            }
        }

        protected abstract void OutputLogMessage(LogLevel level, string message);

        public virtual void DoEnd()
        {
        }
    }
}
