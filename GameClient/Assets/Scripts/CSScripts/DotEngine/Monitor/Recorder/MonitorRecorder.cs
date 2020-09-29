using DotEngine.Monitor.Sampler;
using System;

namespace DotEngine.Monitor.Recorder
{
    public interface IMonitorRecorder: IDisposable
    {
        void DoInit();
        void DoUpdate(float deltaTime);
        void HandleRecords(MonitorSamplerType type, MonitorRecord[] records);
    }

    public enum MonitorRecorderType
    {
        None,
        Console,
        File,
        Profiler,
        Remote,
    }

    public abstract class MonitorRecorder : IMonitorRecorder
    {
        public abstract MonitorRecorderType Type { get; }

        protected MonitorRecorder()
        {
        }

        public virtual void DoInit()
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
