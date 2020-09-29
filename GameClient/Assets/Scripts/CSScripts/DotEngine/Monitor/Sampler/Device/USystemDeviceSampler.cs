using System;
using UnityEngine;

namespace DotEngine.Monitor.Sampler
{
    public class USystemDeviceRecord : MonitorRecord
    {
        public float BatteryLevel { get; set; }
        public BatteryStatus BatteryStatus { get; set; }
        public string DeviceName { get; set; }
        public DeviceType DeviceType { get; set; }
    }

    public class USystemDeviceSampler : MonitorSampler<USystemDeviceRecord>
    {
        public USystemDeviceSampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
        }

        protected override MonitorSamplerType Type => MonitorSamplerType.USystemDevice;

        protected override bool OnSample(USystemDeviceRecord record)
        {
            record.BatteryLevel = SystemInfo.batteryLevel;
            record.BatteryStatus = SystemInfo.batteryStatus;
            record.DeviceName = SystemInfo.deviceName;
            record.DeviceType = SystemInfo.deviceType;

            return true;
        }
    }
}
