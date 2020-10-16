#if UNITY_ANDROID

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfAndroidProcessorMetric : GPerfProcessorMetric
    {
        public override float GetProcessorUsageRate()
        {
            return GPerfPlatform.GetCPUUsageRate();
        }
    }
}

#endif