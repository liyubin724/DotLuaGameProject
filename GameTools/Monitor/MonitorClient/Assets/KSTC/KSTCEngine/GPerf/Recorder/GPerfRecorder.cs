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
       
        void SetUserInfo();

        void DoStart();
        void DoUpdate(float deltaTime);
        void DoEnd();
        void DoDispose();

        void HandleRecord(Record record);
    }

    public abstract class GPerfRecorder : IRecorder
    {
        public RecorderType Type { get; protected set; }

        public void SetUserInfo() { }

        public virtual void DoStart()
        {
        }

        public virtual void DoUpdate(float deltaTime)
        {

        }

        public virtual void DoEnd()
        {

        }

        public abstract void HandleRecord(Record record);

        public virtual void DoDispose()
        {
        }
    }
}
