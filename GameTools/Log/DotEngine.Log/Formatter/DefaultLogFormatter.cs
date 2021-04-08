using System;

namespace DotEngine.Log
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public string FormatMessage(LogLevel level, string tag, string message, string stackTrace)
        {
            return $"{DateTime.Now.ToString("yy-MM-dd HH: mm:ss-fff")} {level.ToString().ToUpper()} {tag} {message} {stackTrace}";
        }
    }
}
