using DotEngine.Pool;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Monitor
{
    public abstract class MonitorRecord : IObjectPoolItem
    {
        public DateTime Time { get; set; }

        public MonitorRecord() { }

        public void OnGet()
        {
            Time = DateTime.Now;
        }

        public void OnNew()
        {
        }

        public void OnRelease()
        {
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }
    }

    public abstract class MonitorSampler<R> where R:MonitorRecord,new()
    {
        protected ObjectPool<R> recordPool = new ObjectPool<R>();

        public int SamplingFrameCount { get; set; } = 60;
        public float SyncIntervalTime { get; set; } = 3.0f;

        protected List<R> cachedRecords = new List<R>();
        
        private int m_ElapseSamplingFrame = 0;
        private float m_ElapseSyncTime = 0.0f;

        public MonitorSampler()
        {
        }

        public void DoUpdate(float deltaTime)
        {

        }

    }
}
