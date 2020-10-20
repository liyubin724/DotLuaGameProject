using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class DeviceRecord : Record
    {
        public string Model { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
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

        protected override void OnSample(DeviceRecord record)
        {
            record.Model = GetModel();
            record.Name = GetName();
            record.UniqueIdentifier = GetUniqueIdentifier();
        }
    }
}
