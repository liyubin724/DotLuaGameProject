using UnityEngine;

namespace DotEngine.Log
{
    public class UnityConsoleAppender : ALogAppender
    {
        public static readonly string NAME = "UnityConsoleAppender";
        public UnityConsoleAppender(ILogFormatter formatter) : base(NAME, formatter)
        {
        }

        public UnityConsoleAppender() : this(new DefaultLogFormatter())
        {
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            if(level>= LogLevel.Error)
            {
                Debug.LogError(message);
            }else if(level>= LogLevel.Warning)
            {
                Debug.LogWarning(message);
            }else
            {
                Debug.Log(message);
            }
        }
    }
}
