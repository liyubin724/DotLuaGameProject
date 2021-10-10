using System;

namespace DotEngine.Log
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public string FormatMessage(string tag, LogLevel level, DateTime dateTime, string message, string stacktrace)
        {
            return $"{DateTime.Now.ToString("MM-dd HH:mm:ss-fff")} {level.ToString().ToUpper()} {tag} {message} {stacktrace}";
        }
    }
}
