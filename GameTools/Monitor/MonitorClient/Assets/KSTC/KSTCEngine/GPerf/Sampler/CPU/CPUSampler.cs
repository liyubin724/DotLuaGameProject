using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class CPURecord:Record
    {
        public float UsageRate { get; set; } = 0.0f;
        public float Frequency { get; set; } = 0.0f;
        public int CoreCount { get; set; } = 1;
    }

    public class CPUSampler : GPerfSampler<CPURecord>
    {
        public CPUSampler() : base()
        {
            MetricType = SamplerMetricType.CPU;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 1.0f;
        }

        public float GetUsageRate()
        {
            return GPerfPlatform.GetCPUUsageRate();
        }

        public float GetFrequency()
        {
            return SystemInfo.processorFrequency;
        }

        public int GetCoreCount()
        {
            return SystemInfo.processorCount;
        }

        protected override void OnSample()
        {
            record.CoreCount = GetCoreCount();
            record.Frequency = GetFrequency();
            record.UsageRate = GetUsageRate();
        }
    }
}
