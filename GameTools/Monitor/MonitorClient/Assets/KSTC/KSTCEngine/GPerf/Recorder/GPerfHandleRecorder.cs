using KSTCEngine.GPerf.Sampler;

namespace KSTCEngine.GPerf.Recorder
{
    public enum RecorderType
    {
        None = 0,
        Console,
        File,
        Remote,
    }

    public interface IRecorder
    {
        RecorderType Type { get; }
       
        void DoStart();
        void DoEnd();
        void DoDispose();
    }

    public interface IIntervalRecorder : IRecorder
    {
        float RecordInterval { get; set; }

        void DoRecord();
        void DoUpdate(float deltaTime);
    }

    public interface IHandleRecorder : IRecorder
    {
        void HandleRecord(Record record);
    }

    public abstract class GPerfRecorder : IRecorder
    {
        public RecorderType Type { get; private set; }

        protected GPerfRecorder(RecorderType type)
        {
            Type = type;
        }

        public virtual void DoStart()
        {
        }
        public virtual void DoEnd()
        {
        }
        public virtual void DoDispose()
        {
        }
    }

    public abstract class GPerfIntervalRecorder : GPerfRecorder, IIntervalRecorder
    {
        public float RecordInterval { get; set; } = 1.0f;

        private float m_ElapsedTime = 0.0f;

        protected GPerfIntervalRecorder(RecorderType type) : base(type)
        {
        }

        public abstract void DoRecord();

        public void DoUpdate(float deltaTime)
        {
            m_ElapsedTime += deltaTime;
            if(m_ElapsedTime>= RecordInterval)
            {
                m_ElapsedTime -= RecordInterval;

                DoRecord();
            }
        }
    }
    public abstract class GPerfHandleRecorder : GPerfRecorder, IHandleRecorder
    {
        protected GPerfHandleRecorder(RecorderType type) : base(type)
        {
        }

        public abstract void HandleRecord(Record record);
    }
}
