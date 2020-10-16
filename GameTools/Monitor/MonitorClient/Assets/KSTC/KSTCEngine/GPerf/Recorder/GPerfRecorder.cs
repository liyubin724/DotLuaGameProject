using KSTCEngine.GPerf.Sampler;

namespace KSTCEngine.GPerf.Recorder
{
    public enum RecorderType
    {
        None,
        Console,
        File,
        Profiler,
        Remote,
    }

    public interface IRecorder
    {
        RecorderType Type { get; }
       
        void SetUserInfo();

        void DoInit();
        void DoDispose();

        void HandleRecords(Record[] records);
    }

    public abstract class GPerfRecorder : IRecorder
    {
        public abstract RecorderType Type { get; }

        public void SetUserInfo() { }

        public virtual void DoInit()
        {
        }

        public abstract void HandleRecords(Record[] records);

        public virtual void DoDispose()
        {

        }
    }
}
