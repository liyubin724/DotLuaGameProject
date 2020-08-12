using Newtonsoft.Json;
using System;
using UnityEngine;

namespace DotEngine.Debugger.Log
{
    public class LogRecord
    {
        public DateTime time;
        public LogType type;
        public string message;
        public string stackTrace;

        public static string ToJson(LogRecord record)
        {
            return JsonConvert.SerializeObject(record, Formatting.None);
        }

        public static LogRecord FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LogRecord>(json);
        }

        public override string ToString()
        {
            return $"[time:{time},logType:{type},message:{message},stackTrace:{stackTrace}]";
        }
    }
}
