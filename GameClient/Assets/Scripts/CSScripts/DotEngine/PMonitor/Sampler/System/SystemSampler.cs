using DotEngine.PMonitor.Recorder;
using UnityEngine;

namespace DotEngine.PMonitor.Sampler
{
    public class SystemSampler : ASampler<SystemRecord>
    {
        public SystemSampler(IRecorder recorder) : base(SamplerCategory.System, recorder)
        {
            SamplingFrameRate = -1;
        }

        protected override void DoSample(SystemRecord data)
        {
            data.BatteryLevel = SystemInfo.batteryLevel;
            data.BatteryStatus = SystemInfo.batteryStatus;
            data.DeviceName = SystemInfo.deviceName;
            data.DeviceType = SystemInfo.deviceType;
        }

        protected override void Init()
        {
        }
    }
}
