using System;

namespace DotEngine.PMonitor.Sampler
{
    public interface ISampler : IDisposable
    {
        void Init();
        void DoUpdate(float deltaTime);
        void Sample();
        void SyncRecord();
    }
}
