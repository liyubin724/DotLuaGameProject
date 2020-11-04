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

            GetDataAsync();
        }

        private async void GetDataAsync()
        {
            var task = Task.Run(() =>
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                AndroidJNI.AttachCurrentThread();
                float temp = GPerfPlatform.GetCPUUsageRate();
                AndroidJNI.DetachCurrentThread();
#else
                float temp = GPerfPlatform.GetCPUUsageRate();
#endif
                return temp;
            });
            record.UsageRate = await task;

            var coreFreqTask = Task.Run(() =>
            {
                long[] temp;
#if !UNITY_EDITOR && UNITY_ANDROID
                AndroidJNI.AttachCurrentThread();
                temp = GPerfPlatform.GetCPUCoreFrequence();
                AndroidJNI.DetachCurrentThread();
#else
                temp = GPerfPlatform.GetCPUCoreFrequence();
#endif
                return temp;
            });
            long[] coreFreq = await coreFreqTask;
            if(coreFreq!=null && coreFreq.Length>0)
            {
                record.CoreFrequency = new int[coreFreq.Length];
                for(int i =0;i<coreFreq.Length;++i)
                {
                    record.CoreFrequency[i] = (int)(coreFreq[i] / 1000);
                }
            }
        }
    }
}
