using KSTCEngine.GPerf.Metric;

namespace KSTCEngine.GPerf.Sampler
{
    public class MemorySampler : GPerfSampler
    {
        private GPerfMemoryMetric m_MemoryMetric = null;
        public MemorySampler() : base()
        {
#if UNITY_ANDROID
            m_MemoryMetric = new GPerfAndroidMemoryMetric();
#elif UNITY_IPHONE

#endif
        }

        public override SamplerType Type => SamplerType.Memory;

        protected override bool Sample(Record record)
        {
            if (m_MemoryMetric != null)
            {
                record.Data = m_MemoryMetric.GetMetricInfo();
                return true;
            }
            return false;
        }
    }
}
