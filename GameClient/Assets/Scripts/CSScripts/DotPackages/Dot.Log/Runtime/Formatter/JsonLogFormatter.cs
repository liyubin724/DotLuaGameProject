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

        public string FormatMessage(LogLevel level, string tag, string message, string stackTrace)
        {
            jsonObj["Level"] = level.ToString().ToUpper();
            jsonObj["Tag"] = tag;
            jsonObj["Message"] = message;
            jsonObj["StackTrace"] = stackTrace;
            jsonObj["Time"] = DateTime.Now.ToString("yy-MM-dd HH: mm:ss: ffff");

            return jsonObj.ToString();
        }
    }
}
