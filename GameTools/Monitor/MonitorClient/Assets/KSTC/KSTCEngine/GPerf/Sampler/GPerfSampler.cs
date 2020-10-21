using System;

namespace KSTCEngine.GPerf.Sampler
{
    public class Record
    {
        public SamplerMetricType MetricType { get; internal set; }
        //public DateTime Time { get; set; }
        //public int FrameIndex { get; set; }

        public String ExtensionData { get; set; }= string.Empty;

        public Record() { }
    }

    public abstract class GPerfSampler<T> : ISampler where T : Record,new()
    { 
        public SamplerMetricType MetricType { get; protected set; }
        public SamplerFreqType FreqType { get; protected set; }
        public float SamplingInterval { get; set; } = 1.0f;
        public Action<Record> OnSampleRecord { get; set; }

        protected T record { get; set; }
        private float m_ElapsedTime = 0.0f;

        public Record GetRecord()
        {
            return record;
        }

        public void DoStart()
        {
            record = new T();
            OnStart();
        }

        public void DoSample()
        {
            record.MetricType = MetricType;
            //RecordData.FrameIndex = Time.frameCount;
            //RecordData.Time = DateTime.Now;
            record.ExtensionData = string.Empty;

            OnSample();

            OnSampleRecord?.Invoke(record);
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
            if(FreqType == SamplerFreqType.End)
            {
                DoSample();
            }

            OnEnd();
        }


        public void DoDispose()
        {
            OnDispose();
        }

        protected virtual void OnStart() { }
        protected abstract void OnSample();
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnEnd() { }
        protected virtual void OnDispose() { }
    }
}
