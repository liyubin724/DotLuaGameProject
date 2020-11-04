using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class BatteryRecord : Record
    {
        public float Temperature { get; set; } = 0.0f;
        public int Status { get; set; } = 0;
        public string StatusDesc { get; set; } = string.Empty;
        public float Rate { get; set; } = 0.0f;
    }

    public class BatterySampler : GPerfSampler<BatteryRecord>
    {
        private Dictionary<BatteryStatus, string> m_StatusDescDic = new Dictionary<BatteryStatus, string>();
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
#if !UNITY_EDITOR && UNITY_ANDROID
                AndroidJNI.AttachCurrentThread();
                float temp = GPerfPlatform.GetBatteryTemperature();
                AndroidJNI.DetachCurrentThread();
#else
                float temp = GPerfPlatform.GetBatteryTemperature();
#endif

                return temp;
            });
            record.Temperature = await task;
        }

        public int GetStatus()
        {
            return (int)SystemInfo.batteryStatus;
        }

        public string GetStatusDesc()
        {
            BatteryStatus bs = SystemInfo.batteryStatus;
            if(!m_StatusDescDic.TryGetValue(bs,out string desc))
            {
                desc = bs.ToString();
                m_StatusDescDic.Add(bs, desc);
            }
            return desc;
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
            record.StatusDesc = GetStatusDesc();
            //GetTemperatureAsync();
        }
    }
}
