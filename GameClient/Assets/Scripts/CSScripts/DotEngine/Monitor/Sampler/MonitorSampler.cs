using DotEngine.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Monitor.Sampler
{
    public class MonitorRecord
    {
        public int FrameIndex { get; set; }
        public DateTime Time { get; set; }

        public MonitorRecord() { }
    }

    public enum MonitorSamplerType
    {
        None = 0,
        FPS,
        Log,
        USystemDevice,
        //ProcSystemDevice,
        ProfilerMemory,
        ProcMemory,
        XLuaMemory,
    }

    public interface IMonitorSampler : IDisposable
    {
        void DoUpdate(float deltaTime);
    }

    public abstract class MonitorSampler<R> : IMonitorSampler where R : MonitorRecord, new()
    {
        protected SimpleObjectPool<R> recordPool = null;

        protected abstract MonitorSamplerType Type { get; }

        public int SamplingFrameCount { get; set; } = 60;
        public float SyncIntervalTime { get; set; } = 3.0f;

        private List<R> m_CachedRecords = new List<R>();

        private int m_ElapseSamplingFrame = 0;
        private float m_ElapseSyncTime = 0.0f;

        private Action<MonitorSamplerType, MonitorRecord[]> m_RecordHandleAction = null;

        protected MonitorSampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction)
        {
            recordPool = new SimpleObjectPool<R>((r) =>
            {
                r.Time = DateTime.Now;
                r.FrameIndex = Time.frameCount;
            }, null);
            m_RecordHandleAction = handleAction;
        }

        public void DoUpdate(float deltaTime)
        {
            OnUpdate(deltaTime);

            if (SamplingFrameCount > 0)
            {
                m_ElapseSamplingFrame++;
                if (m_ElapseSamplingFrame % SamplingFrameCount == 0)
                {
                    Sample();
                    m_ElapseSamplingFrame = 0;
                }
            }
            else
            {
                Sample();
            }

            if (SyncIntervalTime > 0)
            {
                m_ElapseSyncTime += deltaTime;
                if (m_ElapseSyncTime >= SyncIntervalTime)
                {
                    SyncRecord();
                    m_ElapseSyncTime -= SyncIntervalTime;
                }
            }
            else
            {
                SyncRecord();
            }
        }

        protected virtual void OnUpdate(float deltaTime)
        {
        }

        public void Sample()
        {
            R record = recordPool.Get();
            if(OnSample(record))
            {
                m_CachedRecords.Add(record);
            }else
            {
                recordPool.Release(record);
            }
        }

        protected abstract bool OnSample(R record);

        public void SyncRecord()
        {
            if(m_CachedRecords.Count>0)
            {
                R[] records = m_CachedRecords.ToArray();
                m_RecordHandleAction?.Invoke(Type, records);

                foreach(var r in records)
                {
                    recordPool.Release(r);
                }

                m_CachedRecords.Clear();
            }
        }

        public virtual void Dispose()
        {

        }
    }
}
