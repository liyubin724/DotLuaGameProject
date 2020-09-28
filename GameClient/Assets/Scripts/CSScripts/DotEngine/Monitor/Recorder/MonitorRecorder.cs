using DotEngine.Monitor.Sampler;
using System;

namespace DotEngine.Monitor.Recorder
{
    public interface IMonitorRecorder: IDisposable
    {
        void DoUpdate(float deltaTime);
        void HandleRecords(MonitorSamplerType type, MonitorRecord[] records);
    }

    public enum MonitorRecorderType
    {
        None,
        File,
        Console,
        Remote,
        Profiler,
    }

    public abstract class MonitorRecorder : IMonitorRecorder
    {
        public abstract MonitorRecorderType Type { get; }

        protected MonitorRecorder()
        {
        }

        protected virtual void DoInited()
        {

        }

        public abstract void HandleRecords(MonitorSamplerType type, MonitorRecord[] records);

        public virtual void DoUpdate(float deltaTime)
        {

        }

        public virtual void Dispose()
        {

        }
    }
}
