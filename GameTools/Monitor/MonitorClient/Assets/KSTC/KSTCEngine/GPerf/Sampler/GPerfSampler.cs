using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class Record
    {
        public SamplerMetricType Type { get; internal set; }
        public DateTime Time { get; set; }
        public int FrameIndex { get; set; }

        public String ExtensionData { get; set; }= string.Empty;

        public Record() { }
    }

    public abstract class GPerfSampler<T> : ISampler where T : Record,new()
    { 
        public SamplerMetricType MetricType { get; protected set; }
        public SamplerFreqType FreqType { get; protected set; }
        public float SamplingInterval { get; set; } = 1.0f;
        public Action<Record> OnSampleRecord { get; set; }

        protected T RecordData { get; set; }
        private float m_ElapsedTime = 0.0f;

        public Record GetRecord()
        {
            return RecordData;
        }

        public void DoStart()
        {
            RecordData = new T();
            OnStart();
        }

        public void DoSample()
        {
            RecordData.Type = MetricType;
            RecordData.FrameIndex = Time.frameCount;
            RecordData.Time = DateTime.Now;
            RecordData.ExtensionData = string.Empty;

            OnSample(RecordData);

            OnSampleRecord?.Invoke(RecordData);
        }

        public virtual void DoUpdate(float deltaTime)
        {
            OnUpdate(deltaTime);

            if(FreqType == SamplerFreqType.Interval)
            {
                m_ElapsedTime += deltaTime;
                if (m_ElapsedTime >= SamplingInterval)
                {
                    m_ElapsedTime -= SamplingInterval;

                    DoSample();
                }
            }
        }

        public void DoEnd()
        {
            OnEnd();
        }


        public void DoDispose()
        {
            OnDispose();
        }

        protected virtual void OnStart() { }
        protected abstract void OnSample(T record);
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnEnd() { }
        protected virtual void OnDispose() { }
    }
}
