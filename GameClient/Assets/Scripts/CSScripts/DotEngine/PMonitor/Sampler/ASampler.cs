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

        protected ObjectPool<T> recordPool = null;
        protected List<T> cachedRecords = null;

        protected IRecorder recorder = null;

        protected ASampler(SamplerCategory category,IRecorder recorder)
        {
            Category = category;
            this.recorder = recorder;
            recordPool = new ObjectPool<T>();
            cachedRecords = new List<T>();

            Init();
        }

        protected abstract void Init();

        public void DoUpdate(float deltaTime)
        {
            if(SamplingFrameRate>0)
            {
                ++samplingFrame;
                if(samplingFrame%SamplingFrameRate == 0)
                {
                    cachedRecords.Add(Sample());
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

            Update(deltaTime);
        }

        protected virtual void Update(float deltaTime)
        {

        }

        public T Sample()
        {
            T data = recordPool.Get();

            DoSample(data);

            return data;
        }

        protected abstract void DoSample(T data);

        public void SyncRecord()
        {
            if(cachedRecords.Count>0)
            {
                T[] records = cachedRecords.ToArray();
                recorder?.HandleRecord(Category, cachedRecords.ToArray());
                cachedRecords.Clear();

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
