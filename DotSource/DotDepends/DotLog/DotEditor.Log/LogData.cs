using System;
using LogLevel = DotEngine.Log.LogLevel;

namespace DotEditor.Log
{
    public class LogData
    {
        public DateTime Time;
        public string Tag;
        public LogLevel Level;
        public string Message;
        public string Stacktrace;

        public override string ToString()
        {
            return $"[{Time.ToString("yyyy-MM-dd HH:mm:ss FFF")}] [{Tag}] [{Level.ToString().ToUpper()}] {Message}";
        }
    }
}
