using System;

namespace DotEngine.Log
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public bool ShowDateTime { get; set; } = true;
        public string FormatMessage(string tag, LogLevel level, string message)
        {
            if (ShowDateTime)
            {
                return $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss FFF")}] [{level.ToString().ToUpper()}] [{tag}] {message}";
            }
            else
            {
                return $"{level.ToString().ToUpper()} {tag} {message}";
            }
        }
    }
}
