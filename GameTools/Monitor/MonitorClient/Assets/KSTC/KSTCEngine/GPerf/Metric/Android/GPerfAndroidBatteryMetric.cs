#if UNITY_ANDROID

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfAndroidBatteryMetric : GPerfBatteryMetric
    {
        public override float GetTemperature()
        {
            return GPerfPlatform.GetBatteryTemperature();
        }
    }
}
#endif