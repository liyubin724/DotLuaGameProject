using DotEngine.PMonitor.Recorder;
using UnityEngine;

namespace DotEngine.PMonitor.Sampler
{
    public class LogSampler : ASampler<LogRecord>
    {
        public LogSampler(IRecorder recorder) : base(SamplerCategory.Log, recorder)
        {
            SamplingFrameRate = -1;
        }

        protected override void DoSample(LogRecord data)
        {
        }

        public override void Init()
        {
            Application.logMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(string message, string stackTrace, LogType type)
        {
            LogRecord record = recordPool.Get();
            
            record.Type = type;
            record.Message = message;
            record.StackTrace = stackTrace;

            cachedRecords.Add(record);
        }

        public override void Dispose()
        {
            Application.logMessageReceived -= OnMessageReceived;
        }
    }
}
