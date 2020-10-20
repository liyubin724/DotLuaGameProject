using System;
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
        public override SamplerType SamplerType => SamplerType.Battery;

        public BatterySampler() : base()
        {
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

        protected override void Sampling(BatteryRecord record)
        {
            record.Temperature = GetTemperature();
            record.Status = GetStatus();
            record.Rate = GetRate();
        }
    }
}
