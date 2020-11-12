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

        public void OnLogReceived(LogLevel level, string tag, string message)
        {
            string logMessage = logFormatter.FormatMessage(level, tag, message);
            DoLogMessage(level, logMessage);
        }

        protected abstract void DoLogMessage(LogLevel level,string message);

        public void Dispose()
        {
            
        }

        protected virtual void OnDisposed()
        {

        }
    }
}
