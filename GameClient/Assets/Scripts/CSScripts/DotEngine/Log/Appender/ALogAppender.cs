using DotEngine.Log.Formatter;
using System;

namespace DotEngine.Log.Appender
{
    public abstract class ALogAppender : IDisposable
    {
        public string Name { get; protected set; }
        protected ILogFormatter logFormatter;

        private bool m_IsDisposed = false;
        protected ALogAppender(string name,ILogFormatter formatter)
        {
            Name = name;
            logFormatter = formatter;
        }

        ~ALogAppender()
        {
            Dispose(false);
        }

        public void OnLogReceived(LogLevel level, string tag, string message)
        {
            string logMessage = logFormatter.FormatMessage(level, tag, message);
            DoLogMessage(level, logMessage);
        }

        protected abstract void DoLogMessage(LogLevel level,string message);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_IsDisposed) return;
            if(disposing)
            {
                OnDisposed();
            }
            m_IsDisposed = true;
        }

        protected virtual void OnDisposed()
        {

        }
    }
}
