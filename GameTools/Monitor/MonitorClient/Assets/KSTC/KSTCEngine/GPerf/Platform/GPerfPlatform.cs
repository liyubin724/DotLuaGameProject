using KSTCEngine.GPerf.Platform;

namespace KSTCEngine.GPerf
{
    public static class GPerfPlatform
    {
        private static GPerfPlatformPlugin sm_PlatformPlugin = null;

        public static void InitPlugin()
        {
#if UNITY_EDITOR

#if UNITY_EDITOR_WIN
            sm_PlatformPlugin = new GPerfWinPlugin();
#endif

#else

#if UNITY_ANDROID
            sm_PlatformPlugin = new GPerfAndroidPlugin();
#elif UNITY_IPHONE

#elif UNITY_STANDALONE_WIN
            sm_PlatformPlugin = new GPerfWinPlugin();
#endif

#endif


        }

        public static float GetCPUUsageRate()
        {
            if(sm_PlatformPlugin!=null)
            {
                return sm_PlatformPlugin.GetCPUUsageRate();
            }
            return 0.0f;
        }

        public static long[] GetCPUCoreFrequence()
        {
            if (sm_PlatformPlugin != null)
            {
                return sm_PlatformPlugin.GetCPUCoreFrequence();
            }
            return new long[0];
        }

        public static string GetMemoryInfo()
        {
            if (sm_PlatformPlugin != null)
            {
                return sm_PlatformPlugin.GetMemoryInfo();
            }
            return string.Empty;
        }

        public static long GetMemoryPSS()
        {
            if(sm_PlatformPlugin != null)
            {
                return sm_PlatformPlugin.GetPSSMemory();
            }
            return 0L;
        }

        public static float GetBatteryTemperature()
        {
            if (sm_PlatformPlugin != null)
            {
                return sm_PlatformPlugin.GetBatteryTemperature();
            }
            return 0.0f;
        }

        public static string GetBatteryInfo()
        {
            if (sm_PlatformPlugin != null)
            {
                return sm_PlatformPlugin.GetBatteryInfo();
            }
            return string.Empty;
        }

    }
}
