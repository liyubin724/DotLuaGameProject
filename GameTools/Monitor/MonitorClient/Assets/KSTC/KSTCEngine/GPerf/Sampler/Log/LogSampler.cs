using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class LogRecord : Record
    {
        public string Condition { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public LogType LogType { get; set; } = LogType.Log;
    }

    public class LogSampler : GPerfSampler<LogRecord>
    {
        public LogSampler()
        {
            MetricType = SamplerMetricType.Log;
            FreqType = SamplerFreqType.End;
        }

        protected override void OnStart()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        protected override void OnSample()
        {
        }

        protected override void OnDispose()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {

        }
    }
}
