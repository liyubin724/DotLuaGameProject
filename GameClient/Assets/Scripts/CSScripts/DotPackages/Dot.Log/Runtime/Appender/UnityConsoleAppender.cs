using UnityEngine;

namespace DotEngine.Log
{
    public class UnityConsoleAppender : ALogAppender
    {
        public UnityConsoleAppender(ILogFormatter formatter) : base(typeof(UnityConsoleAppender).Name, formatter)
        {
        }

        public UnityConsoleAppender() : this(new DefaultLogFormatter())
        {
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            if (level <= LogLevel.Info)
            {
                Debug.Log(message);
            }
            else if (level == LogLevel.Warning)
            {
                Debug.LogWarning(message);
            }
            else if (level >= LogLevel.Error)
            {
                Debug.LogError(message);
            }
        }
    }
}
