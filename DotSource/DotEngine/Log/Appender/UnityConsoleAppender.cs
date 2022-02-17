using UnityEngine;

namespace DotEngine.Log
{
    public class UnityConsoleAppender : LogAppender
    {
        public static readonly string NAME = "UnityConsoleAppender";
        public UnityConsoleAppender(ILogFormatter formatter = null) : base(NAME, formatter)
        {
        }

        protected override void OutputMessage(string tag, LogLevel level, string formattedMessage)
        {
            if (level >= LogLevel.Error)
            {
                Debug.LogError(formattedMessage);
            }
            else if (level >= LogLevel.Warning)
            {
                Debug.LogWarning(formattedMessage);
            }
            else
            {
                Debug.Log(formattedMessage);
            }
        }
    }
}
