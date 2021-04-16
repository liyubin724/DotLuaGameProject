using System;

namespace DotEngine.Log
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public string FormatMessage(LogLevel level, string tag,DateTime dateTime, string message, string stackTrace)
        {
            return $"{dateTime.ToString("yy-MM-dd HH:mm:ss-fff")} {level.ToString().ToUpper()} {tag} {message} {stackTrace}";
        }
    }
}
