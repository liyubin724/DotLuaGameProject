using DotEngine.Log.Formatter;
using System;

namespace DotEngine.Log.Appender
{
    public abstract class ALogAppender : IDisposable
    {
        public string Name { get; protected set; }
        protected ILogFormatter logFormatter;

        protected ALogAppender(string name,ILogFormatter formatter)
        {
            Name = name;
            logFormatter = formatter;
        }

        public void OnLogReceived(LogLevel level, string tag, string message,string stackTrace)
        {
            string logMessage = logFormatter.FormatMessage(level, tag, message, stackTrace);
            DoLogMessage(level, logMessage);
        }

        protected abstract void DoLogMessage(LogLevel level,string message);

        public virtual void Dispose()
        {
        }
    }
}
