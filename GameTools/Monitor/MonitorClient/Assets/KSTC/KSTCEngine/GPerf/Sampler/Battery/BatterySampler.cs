using System.Threading.Tasks;
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

        private async void GetTemperatureAsync()
        {
            var task = Task.Run(() =>
            {
                AndroidJNI.AttachCurrentThread();
                float temp = GPerfPlatform.GetBatteryTemperature();
                AndroidJNI.DetachCurrentThread();
                return temp;
            });
            record.Temperature = await task;
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
            record.Temperature = GetTemperature();
            //GetTemperatureAsync();
        }
    }
}
