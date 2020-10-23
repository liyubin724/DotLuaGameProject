using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class DeviceRecord : Record
    {
        public string Name { get; set; } = string.Empty;//用户定义的设备名称
        public string Model { get; set; } = string.Empty;//设备型号
        public string UniqueIdentifier { get; set; } = string.Empty;
    }

    public class DeviceSampler : GPerfSampler<DeviceRecord>
    {
        public DeviceSampler()
        {
            MetricType = SamplerMetricType.Device;
            FreqType = SamplerFreqType.Start;
        }

        public string GetModel()
        {
            return SystemInfo.deviceModel;
        }

        public string GetName()
        {
            return SystemInfo.deviceName;
        }

        public string GetUniqueIdentifier()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        protected override void OnStart()
        {
            DoSample();
        }

        protected override void OnSample()
        {
            record.Model = GetModel();
            record.Name = GetName();
            record.UniqueIdentifier = GetUniqueIdentifier();
        }
    }
}
