using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Monitor.Sampler
{
    public class LogData
    {
        public LogType Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public class LogRecord : MonitorRecord
    {
        public LogData[] Datas { get; set; }
    }

    public class LogSampler : MonitorSampler<LogRecord>
    {
        private List<LogData> m_LogDatas = new List<LogData>();

        protected override MonitorSamplerType Type => MonitorSamplerType.Log;

        public LogSampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
            Application.logMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(string message, string stackTrace, LogType type)
        {
            m_LogDatas.Add(new LogData()
            {
                Type = type,
                Message = message,
                StackTrace = stackTrace,
            });
        }

        protected override void OnSample(LogRecord record)
        {
            record.Datas = m_LogDatas.ToArray();
            m_LogDatas.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            Application.logMessageReceived -= OnMessageReceived;
        }
    }
}
