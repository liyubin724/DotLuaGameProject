using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace KSTCEngine.GPerf.Sampler
{
    public class Record
    {
        public SamplerMetricType MetricType { get; internal set; }
        public String ExtensionData { get; set; }= string.Empty;

        public Record() { }

        public override string ToString()
        {
            StringBuilder descSB = new StringBuilder();

            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var p in propertyInfos)
            {
                descSB.Append($"{p.Name}:{p.GetValue(this)}; ");
            }

            return descSB.ToString();
        }
    }

    public abstract class GPerfSampler<T> : ISampler where T : Record,new()
    { 
        public SamplerMetricType MetricType { get; protected set; }
        public SamplerFreqType FreqType { get; protected set; }
        public Action<Record> OnSampleRecord { get; set; }

        protected T record { get; set; }
        public Record GetRecord()
        {
            return record;
        }

        public void DoStart()
        {
            record = new T();
            record.MetricType = MetricType;
            OnStart();

            if(FreqType == SamplerFreqType.Start || FreqType == SamplerFreqType.Interval)
            {
                DoSample();
            }
        }

        public void DoSample()
        {
            record.ExtensionData = string.Empty;
            if(Debug.isDebugBuild)
            {
                Profiler.BeginSample($"GPerfSampler:{MetricType}");
                OnSample();
                Profiler.EndSample();
            }
            else
            {
                OnSample();
            }

            OnSampleRecord?.Invoke(record);
        }

        public virtual void DoUpdate(float deltaTime)
        {
            OnUpdate(deltaTime);
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
