using UnityEngine;

namespace DotEngine.Log
{
    public class UnityAppender : ALogAppender
    {
        public static readonly string NAME = "UnityConsole";

        public UnityAppender() : base(NAME)
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