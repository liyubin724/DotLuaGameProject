using System;

namespace DotEngine.Log
{
    public class DefaultLogFormatter : ILogFormatter
    {
        public string FormatMessage(DateTime time, string tag, LogLevel level, string message, string stacktree)
        {
            if(string.IsNullOrEmpty(stacktree))
            {
                return $"[{time.ToString("yyyy-MM-dd HH:mm:ss FFF")}] [{level.ToString().ToUpper()}] [{tag}] {message}";
            }else
            {
                return $"[{time.ToString("yyyy-MM-dd HH:mm:ss FFF")}] [{level.ToString().ToUpper()}] [{tag}] {message} {Environment.NewLine}{stacktree}";
            }
        }
    }
}
