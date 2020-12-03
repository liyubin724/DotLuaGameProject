using Newtonsoft.Json;
using System;
using System.Text;

namespace DotEngine.Log.Formatter
{
    [Serializable]
    public class LogData
    {
        public LogLevel Level { get; set; }
        public long Time { get; set; }
        public string Tag { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Level:      {Level}");
            sb.AppendLine($"Time:      {new DateTime(Time)}");
            sb.AppendLine($"Tag:        {Tag}");
            sb.AppendLine($"Message:{Message}");
            if (!string.IsNullOrEmpty(StackTrace))
            {
                sb.AppendLine($"StackTrace:\n{StackTrace}");
            }
            return sb.ToString();
        }
    }

    public class JsonLogFormatter : ILogFormatter
    {
        private LogData m_LogData = new LogData();
        public JsonLogFormatter()
        {
        }

        public string FormatMessage(LogLevel level, string tag, string message, string stackTrace)
        {
            m_LogData.Level = level;
            m_LogData.Tag = tag;
            m_LogData.Message = message;
            m_LogData.StackTrace = stackTrace;
            m_LogData.Time = DateTime.Now.Ticks;

            return JsonConvert.SerializeObject(m_LogData);
        }
    }
}
