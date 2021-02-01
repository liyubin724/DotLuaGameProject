using DotEngine.Log.Formatter;
using UnityEngine;

namespace DotEngine.Log.Appender
{
    public class UnityConsoleAppender : ALogAppender
    {
        public static readonly string NAME = "UnityConsole";

        public UnityConsoleAppender(ILogFormatter formatter) : base(NAME, formatter)
        {
        }

        public UnityConsoleAppender() : base(NAME, new DefaultLogFormatter())
        {
        }

        protected override void DoLogMessage(LogLevel level, string message)
        {
            if (level <= LogLevel.Info)
            {
                Debug.Log(message);
            }else if(level == LogLevel.Warning)
            {
                Debug.LogWarning(message);
            }else if(level >= LogLevel.Error)
            {
                Debug.LogError(message);
            }
        }
    }
}
