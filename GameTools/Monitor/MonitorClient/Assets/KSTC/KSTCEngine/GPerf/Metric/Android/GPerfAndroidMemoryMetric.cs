#if UNITY_ANDROID
namespace KSTCEngine.GPerf.Metric
{
    public class GPerfAndroidMemoryMetric : GPerfMemoryMetric
    {
        public override string GetMetricInfo()
        {
            return GPerfPlatform.GetMemoryInfo();
        }

        public override long GetPSS()
        {
            return GPerfPlatform.GetMemoryPSS();
        }
    }
}
#endif