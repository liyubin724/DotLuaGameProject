using System.Threading.Tasks;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class CPURecord:Record
    {
        public float UsageRate { get; set; } = 0.0f;
        public int Frequency { get; set; } = 0;
        public int CoreCount { get; set; } = 1;

        public int[] CoreFrequency { get; set; } = new int[0];
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

        private async void GetUsageRateAsync()
        {
            var task = Task.Run(() =>
            {
                AndroidJNI.AttachCurrentThread();
                float temp = GPerfPlatform.GetCPUUsageRate();
                AndroidJNI.DetachCurrentThread();
                return temp;
            });
            record.UsageRate = await task;
        }

        public int GetFrequency()
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

            GetUsageRateAsync();
        }
    }
}
