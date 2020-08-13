using DotEngine.PMonitor.Sampler;
using System;

namespace DotEngine.PMonitor.Recorder
{
    public interface IRecorder:IDisposable
    {
        RecorderCategory Category { get; }
        void Init();
        void HandleRecord(SamplerCategory category, Record[] records);
    }
}
