using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public enum SamplerType
    {
        None = 0,
        FPS,
        Device,
        Memory,
        CPU,
        Battery,
    }

    public class Record
    {
        public SamplerType Type { get; set; }
        public DateTime Time { get; set; }
        public int FrameIndex { get; set; }
        public String Data { get; set; }= string.Empty;

        public Record() { }
    }

    public interface ISampler
    {
        SamplerType Type { get; }
        bool DoSample(Record record);
        void DoUpdate(float deltaTime);
        void DoDispose();
    }

    public abstract class GPerfSampler : ISampler
    {
        public abstract SamplerType Type { get; }
        protected GPerfSampler()
        {
        }

        public bool DoSample(Record record)
        {
            record.Type = Type;
            return Sample(record);
        }

        protected abstract bool Sample(Record record);

        public virtual void DoUpdate(float deltaTime)
        {
        }

        public void DoDispose()
        {
        }

    }
}
