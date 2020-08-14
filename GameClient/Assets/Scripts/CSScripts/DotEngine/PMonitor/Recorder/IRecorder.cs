using DotEngine.PMonitor.Sampler;
using System;

namespace DotEngine.PMonitor.Recorder
{
    public interface IRecorder:IDisposable
    {
        RecorderCategory Category { get; }
        void Init();
        void DoUpdate(float deltaTime);
        void HandleRecord(SamplerCategory category, Record[] records);
    }
}
