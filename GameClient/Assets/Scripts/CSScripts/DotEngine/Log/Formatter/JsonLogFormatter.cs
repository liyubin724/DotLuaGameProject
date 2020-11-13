using Newtonsoft.Json.Linq;
using System;

namespace DotEngine.Log.Formatter
{
    public class JsonLogFormatter : ILogFormatter
    {
        private JObject logJson = new JObject();
        public JsonLogFormatter()
        {
        }

        public string FormatMessage(LogLevel level, string tag, string message, string stackTrace)
        {
            logJson["time"] = DateTime.Now.Ticks;
            logJson["level"] = (int)level;
            logJson["tag"] = tag;
            logJson["message"] = message;
            logJson["stacktrace"] = stackTrace;

            return logJson.ToString();
        }
    }
}
