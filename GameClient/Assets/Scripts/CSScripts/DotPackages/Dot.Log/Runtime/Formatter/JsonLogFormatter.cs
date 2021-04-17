using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace DotEngine.Log
{
    [Serializable]
    public class LogData
    {
        public LogLevel Level { get; set; }
        public string Tag { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public long Time { get; set; }
    }

    public class JsonLogFormatter : ILogFormatter
    {
        private JObject jsonObj = new JObject();
        public JsonLogFormatter()
        {
        }

        public string FormatMessage(LogLevel level, DateTime dateTime, string tag, string message, string stacktrace)
        {
            jsonObj["tag"] = tag;
            jsonObj["time"] = dateTime.Ticks  / 10000;
            jsonObj["level"] = level.ToString().ToUpper();
            jsonObj["message"] = message;
            jsonObj["stackTrace"] = stacktrace;

            return jsonObj.ToString();
        }
    }
}
