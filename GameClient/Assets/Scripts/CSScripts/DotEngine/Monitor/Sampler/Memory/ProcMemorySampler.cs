using System;

namespace DotEngine.Monitor.Sampler
{
    public class ProcMemoryRecord : MonitorRecord
    {
        public string ProcInfo { get; set; }
    }

    public class ProcMemorySampler : MonitorSampler<ProcMemoryRecord>
    {
        public ProcMemorySampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
        }

        protected override MonitorSamplerType Type => MonitorSamplerType.ProcMemory;

        protected override void OnSample(ProcMemoryRecord record)
        {
#if UNITY_ANDROID

#else
            record.ProcInfo = string.Empty;
#endif
        }
    }
}
