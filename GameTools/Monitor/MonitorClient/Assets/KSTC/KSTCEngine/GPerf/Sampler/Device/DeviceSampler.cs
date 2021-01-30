using System;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class DeviceRecord : Record
    {
        public string Name { get; set; } = string.Empty;//用户定义的设备名称
        public string Model { get; set; } = string.Empty;//设备型号
        public string UniqueIdentifier { get; set; } = string.Empty;
        public string GraphicsName { get; set; } = string.Empty;
        public string GraphicsType { get; set; } = string.Empty;
        public string GraphicsVendor { get; set; } = string.Empty;
        public string GraphicsVersion { get; set; } = string.Empty;
        public int GraphicsMemoryInMB { get; set; } = 0;

        public int SystemMemorySize { get; set; } = 0;
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

        public string GetGraphicsName()
        {
            return SystemInfo.graphicsDeviceName;
        }

        public string GetGraphicsType()
        {
            return SystemInfo.graphicsDeviceType.ToString();
        }

        public string GetGraphicsVendor()
        {
            return SystemInfo.graphicsDeviceVendor;
        }

        public string GetGraphicsVersion()
        {
            return SystemInfo.graphicsDeviceVersion;
        }

        public int GetGraphicsMemoryInMB()
        {
            return SystemInfo.graphicsMemorySize;
        }

        public int GetSystemMemoryInMB()
        {
            return SystemInfo.systemMemorySize;
        }

        protected override void OnSample()
        {
            record.Model = GetModel();
            record.Name = GetName();
            record.UniqueIdentifier = GetUniqueIdentifier();

            record.GraphicsName = GetGraphicsName();
            record.GraphicsType = GetGraphicsType();
            record.GraphicsVendor = GetGraphicsVendor();
            record.GraphicsVersion = GetGraphicsVersion();
            record.GraphicsMemoryInMB = GetGraphicsMemoryInMB();

            record.SystemMemorySize = GetSystemMemoryInMB();
        }
    }
}
