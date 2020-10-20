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

    public enum SamplerFrequencyType
    {
        Once = 0,
        Interval,
    }

    public class Record
    {
        public DateTime Time { get; set; }
        public int FrameIndex { get; set; }

        public String ExtensionData { get; set; }= string.Empty;

        public Record() { }
    }

    public interface ISampler
    {
        SamplerType SamplerType { get;}
        SamplerFrequencyType FrequencyType { get; set; }
        float SamplingInterval { get; set; }

        Record GetRecord(); 

        void DoSample();
        void DoUpdate(float deltaTime);
        void DoDispose();
    }

    public abstract class GPerfSampler<T> : ISampler where T : Record,new()
    { 
        public abstract SamplerType SamplerType { get; }
        public SamplerFrequencyType FrequencyType { get; set; } = SamplerFrequencyType.Interval;
        public float SamplingInterval { get; set; } = 1.0f;

        private T m_Record;
        private int m_SamplingCount = 0;
        private float m_ElapsedTime = 0.0f;

        protected GPerfSampler()
        {
            m_Record = new T();
        }

        public Record GetRecord()
        {
            return m_Record;
        }

        public void DoSample()
        {
            ++m_SamplingCount;

            m_Record.FrameIndex = Time.frameCount;
            m_Record.Time = DateTime.Now;
            m_Record.ExtensionData = string.Empty;

            Sampling(m_Record);
        }

        protected abstract void Sampling(T record);

        public virtual void DoUpdate(float deltaTime)
        {
            if(FrequencyType == SamplerFrequencyType.Once && m_SamplingCount == 0)
            {
                DoSample();
            }else if(FrequencyType == SamplerFrequencyType.Interval)
            {
                m_ElapsedTime += deltaTime;
                if (m_ElapsedTime >= SamplingInterval)
                {
                    m_ElapsedTime -= SamplingInterval;

                    DoSample();
                }
            }
        }

        public virtual void DoDispose()
        {
        }
    }
}
