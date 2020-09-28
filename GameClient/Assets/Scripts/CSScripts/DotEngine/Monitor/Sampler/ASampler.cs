using DotEngine.PMonitor.Recorder;
using DotEngine.Pool;
using System.Collections.Generic;

namespace DotEngine.PMonitor.Sampler
{
    public abstract class ASampler<T> : ISampler where T:Record,new()
    {
        public int SamplingFrameRate { get; set; } = 5;
        public float SyncInterval { get; set; } = 1.0f;

        public SamplerCategory Category { get; private set; }

        private int samplingFrame = 0;
        private float syncElapse = 0.0f;

        protected ObjectPool<T> recordPool = new ObjectPool<T>();
        protected List<T> cachedRecords = new List<T>();

        protected IRecorder recorder = null;

        protected ASampler(SamplerCategory category,IRecorder recorder)
        {
            Category = category;
            this.recorder = recorder;
        }

        public virtual void Init()
        {

        }

        public void DoUpdate(float deltaTime)
        {
            if(SamplingFrameRate > 0)
            {
                ++samplingFrame;
                if(samplingFrame%SamplingFrameRate == 0)
                {
                    Sample();
                    samplingFrame = 0;
                }
            }

            if(SyncInterval>0)
            {
                syncElapse += deltaTime;
                if(syncElapse >= SyncInterval)
                {
                    SyncRecord();
                    syncElapse -= SyncInterval;
                }
            }

            InnerUpdate(deltaTime);
        }

        protected virtual void InnerUpdate(float deltaTime)
        {

        }

        public void Sample()
        {
            T data = recordPool.Get();
            DoSample(data);
            cachedRecords.Add(data);
        }

        protected abstract void DoSample(T data);

        public void SyncRecord()
        {
            if(cachedRecords.Count>0)
            {
                T[] records = cachedRecords.ToArray();
                cachedRecords.Clear();

                recorder?.HandleRecord(Category, records);

                foreach(var record in records)
                {
                    recordPool.Release(record);
                }
            }
        }

        public virtual void Dispose()
        {
            
        }
    }
}
