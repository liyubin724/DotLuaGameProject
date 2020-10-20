#if UNITY_ANDROID
using UnityEngine;

namespace KSTCEngine.GPerf
{
    public class GPerfAndroidPlugin : GPerfPlatformPlugin
    {
        private const string PLUGIN_CLASS_NAME = "com.kingsoft.tc.uplugin.PlatformPlugin";
        private const string BATTERY_TEMPERATURE_METHOD = "getBatteryTemperature";
        private const string BATTERY_INFO_METHOD = "getBatteryInfo";

        private const string DEVICE_INFO_METHOD = "getBuildInfo";

        private const string MEMORY_PSS_METHOD = "getMemoryPss";
        private const string MEMORY_INFO_METHOD = "getMemoryInfo";

        private const string CPU_USAGE_RATE_METHOD = "getCPUUsageRate";
        private const string CPU_INFO_METHOD = "getCPUInfo";

        private AndroidJavaClass m_PluginClass = null;

        public GPerfAndroidPlugin()
        {
            m_PluginClass = new AndroidJavaClass(PLUGIN_CLASS_NAME);
            if (m_PluginClass == null)
            {
                Debug.LogError($"{GPerfUtil.LOG_NAME}::GPerfAndroidPlugin->PluginClass not found");
            }
        }

        private R CallPluginStaticMethod<R>(string methodName,R defaultValue)
        {
            if(m_PluginClass!=null)
            {
                return m_PluginClass.CallStatic<R>(methodName);
            }
            return defaultValue;
        }

        private void CallPluginStaticMethod(string methodName)
        {
            if (m_PluginClass != null)
            {
                m_PluginClass.CallStatic(methodName);
            }
        }

        public override float GetBatteryTemperature()
        {
            return CallPluginStaticMethod<float>(BATTERY_TEMPERATURE_METHOD, 0.0f);
        }

        public override string GetBatteryInfo()
        {
            return CallPluginStaticMethod<string>(BATTERY_INFO_METHOD,string.Empty);
        }

        public override string GetDeviceInfo()
        {
            return CallPluginStaticMethod<string>(DEVICE_INFO_METHOD,string.Empty);
        }

        public override long GetPSSMemory()
        {
            return CallPluginStaticMethod<long>(MEMORY_PSS_METHOD, 0L);
        }

        public override string GetMemoryInfo()
        {
            return CallPluginStaticMethod<string>(MEMORY_INFO_METHOD, string.Empty);
        }

        public override float GetCPUUsageRate()
        {
            return CallPluginStaticMethod<float>(CPU_USAGE_RATE_METHOD, 0.0f);
        }

        public override string GetCPUInfo()
        {
            return CallPluginStaticMethod<string>(CPU_INFO_METHOD, string.Empty);
        }
    }
}
#endif