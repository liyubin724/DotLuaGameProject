using UnityEngine;

namespace DotEngine.PMonitor.Sampler
{
    public class SystemRecord : Record
    {
        public float BatteryLevel { get; set; }
        public BatteryStatus BatteryStatus { get; set; }
        public string DeviceName { get; set; }
        public DeviceType DeviceType { get; set; }

        public override void OnNew()
        {
            Category = SamplerCategory.System;
        }
    }
}
