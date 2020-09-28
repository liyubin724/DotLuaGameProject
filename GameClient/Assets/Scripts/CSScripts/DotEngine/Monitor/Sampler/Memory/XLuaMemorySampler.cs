using System;

namespace DotEngine.Monitor.Sampler
{
    public class XLuaMemoryRecord:MonitorRecord
    {
        public float TotalMemory { get; set; } = 0.0f;
    }

    public class XLuaMemorySampler : MonitorSampler<XLuaMemoryRecord>
    {
        public XLuaMemorySampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
        }

        protected override MonitorSamplerType Type => MonitorSamplerType.XLuaMemory;

        protected override void OnSample(XLuaMemoryRecord record)
        {
            
        }
    }
}
