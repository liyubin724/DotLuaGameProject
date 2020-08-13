using System;

namespace DotEngine.PMonitor.Sampler
{
    public interface ISampler : IDisposable
    {
        void DoUpdate(float deltaTime);
    }
}
