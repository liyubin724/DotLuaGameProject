using System;

namespace DotEngine.Log
{
    public abstract class ALogAppender : ILogAppender
    {
        public string Name { get; private set; }

        public LogLevel ValidLevel { get; private set; }
        protected ILogFormatter Formatter { get; set; } = new DefaultLogFormatter();

        public ALogAppender(string name,LogLevel validLevel)
        {
            Name = name;
            ValidLevel = validLevel;
        }

        public virtual void DoStart()
        {
        }

        public void OnLogReceived(LogLevel level, string tag, DateTime dateTime,string message, string stackTrace)
        {
            if((ValidLevel & level)>0)
            {
                string logMessage = Formatter.FormatMessage(level, tag, dateTime,message, stackTrace);
                OutputLogMessage(level, logMessage);
            }
        }

        protected abstract void OutputLogMessage(LogLevel level, string message);

        public virtual void DoEnd()
        {
        }
    }
}
