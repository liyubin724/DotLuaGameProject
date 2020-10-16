namespace KSTCEngine.GPerf.Metric
{
    public class GPerfMemoryMetric : IGPerfMetric
    {
        public const string MEMORY_TOTAL_KEY = "totalMem";
        public const string MEMORY_AVAILABLE_KEY = "availMem";
        public const string MEMORY_THRESHOLD_KEY = "threshold";
        public const string MEMORY_IS_LOW_KEY = "lowMemory";
        public const string MEMORY_PSS_KEY = "PSS";

        public virtual long GetPSS()
        {
            return 0L;
        }

        public virtual string GetMetricInfo()
        {
            return string.Empty;
        }
    }
}
