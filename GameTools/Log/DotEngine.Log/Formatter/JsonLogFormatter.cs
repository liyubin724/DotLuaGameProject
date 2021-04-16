using Newtonsoft.Json.Linq;
using System;

namespace DotEngine.Log
{
    public class JsonLogFormatter : ILogFormatter
    {
        private JObject jsonObj = new JObject();
        private DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0);
        public JsonLogFormatter()
        {
        }

        public string FormatMessage(LogLevel level, string tag,DateTime dateTime, string message, string stackTrace)
        {
            jsonObj["level"] = level.ToString().ToUpper();
            jsonObj["tag"] = tag;
            jsonObj["time"] = (dateTime.Ticks - dt1970.Ticks) / 10000;
            jsonObj["message"] = message;
            jsonObj["stackTrace"] = stackTrace;

            return jsonObj.ToString();
        }
    }
}
