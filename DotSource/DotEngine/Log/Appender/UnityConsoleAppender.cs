using System;
using UnityEngine;

namespace DotEngine.Log
{
    public class UnityConsoleAppender : ALogAppender
    {
        public UnityConsoleAppender(string name) : base(name)
        {
        }

        protected override void OutputMessage(DateTime time, string tag, LogLevel level, string message, string stacktree)
        {
            string formattedMessage;
            if (string.IsNullOrEmpty(stacktree))
            {
                formattedMessage = $"[{time.ToString("yyyy-MM-dd HH:mm:ss FFF")}] [{level.ToString().ToUpper()}] [{tag}] {message}";
            }
            else
            {
                formattedMessage = $"[{time.ToString("yyyy-MM-dd HH:mm:ss FFF")}] [{level.ToString().ToUpper()}] [{tag}] {message} {Environment.NewLine}{stacktree}";
            }

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
