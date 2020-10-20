using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class DeviceRecord : Record
    {
        public string DeviceModel { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string DeviceUniqueIdentifier { get; set; } = string.Empty;
    }

    public class DeviceSampler : GPerfSampler<DeviceRecord>
    {
        public override SamplerType SamplerType => SamplerType.Device;

        public DeviceSampler() : base()
        {
            FrequencyType = SamplerFrequencyType.Once;
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

        protected override void Sampling(DeviceRecord record)
        {
            record.DeviceModel = GetModel();
            record.DeviceName = GetName();
            record.DeviceUniqueIdentifier = GetUniqueIdentifier();
        }
    }
}
