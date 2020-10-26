using System;
using System.Threading;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class BatteryRecord : Record
    {
        public float Temperature { get; set; } = 0.0f;
        public int Status { get; set; } = 0;
        public float Rate { get; set; } = 0.0f;
    }

    public class BatterySampler : GPerfSampler<BatteryRecord>
    {
        public BatterySampler()
        {
            MetricType = SamplerMetricType.Battery;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 1.0f;
        }

        public float GetTemperature()
        {
            return GPerfPlatform.GetBatteryTemperature(); 
        }

        public int GetStatus()
        {
            return (int)SystemInfo.batteryStatus;
        }

        public float GetRate()
        {
            return SystemInfo.batteryLevel;
        }

        protected override void OnSample()
        {
            record.Status = GetStatus();
            record.Rate = GetRate();

            ThreadPool.QueueUserWorkItem((value) =>
            {
                AndroidJNI.AttachCurrentThread();
                {
                    record.Temperature = GPerfPlatform.GetBatteryTemperature();
                }
                AndroidJNI.DetachCurrentThread();
            });
        }
    }
}
