using System;

namespace DotEngine.Log.Formatter
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public string FormatMessage(LogLevel level, string tag, string message)
        {
            return $"{DateTime.Now.ToString("yy-MM-dd HH: mm:ss: ffff")} {level.ToString().ToUpper()} {tag} {message}";
        }
    }
}
