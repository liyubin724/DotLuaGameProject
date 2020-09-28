using DotEngine.PMonitor.Sampler;

namespace DotEngine.PMonitor.Recorder
{
    public abstract class ARecorder : IRecorder
    {
        public RecorderCategory Category { get; private set; }

        protected ARecorder(RecorderCategory category)
        {
            Category = category;
        }

        public virtual void Init()
        {

        }

        public virtual void DoUpdate(float deltaTime)
        {

        }

        public virtual void Dispose()
        {
            
        }

        public abstract void HandleRecord(SamplerCategory category, Record[] records);
    }
}
