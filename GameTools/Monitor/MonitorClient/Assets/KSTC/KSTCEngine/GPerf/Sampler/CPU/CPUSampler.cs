using KSTCEngine.GPerf.Metric;

namespace KSTCEngine.GPerf.Sampler
{
    public class CPUSampler : GPerfSampler
    {
        private GPerfProcessorMetric m_ProcessorMetric = null;
        public CPUSampler() : base()
        {
#if UNITY_ANDROID
            m_ProcessorMetric = new GPerfAndroidProcessorMetric();
#elif UNITY_IPHONE

#endif
        }

        public override SamplerType Type => SamplerType.CPU;

        protected override bool Sample(Record record)
        {
            if (m_ProcessorMetric != null)
            {
                record.Data = m_ProcessorMetric.GetMetricInfo();
                return true;
            }
            return false;
        }
    }
}
