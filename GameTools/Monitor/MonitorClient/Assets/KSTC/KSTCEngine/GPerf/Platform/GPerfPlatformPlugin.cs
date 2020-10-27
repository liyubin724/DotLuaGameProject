namespace KSTCEngine.GPerf
{
    public abstract class GPerfPlatformPlugin
    {
        public virtual float GetBatteryTemperature()
        {
            return 0.0f;
        }

        public virtual string GetBatteryInfo()
        {
            return string.Empty;
        }

        public virtual string GetDeviceInfo()
        {
            return string.Empty;
        }

        public virtual long GetPSSMemory()
        {
            return 0L;
        }

        public virtual string GetMemoryInfo()
        {
            return string.Empty;
        }

        public virtual float GetCPUUsageRate()
        {
            return 0.0f;
        }

        public virtual long[] GetCPUCoreFrequence()
        {
            return new long[0];
        }

        public virtual string GetCPUInfo()
        {
            return string.Empty;
        }
    }
}

