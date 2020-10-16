using KSTCEngine.GPerf.Metric;

namespace KSTCEngine.GPerf.Sampler
{
    public class BatterySampler : GPerfSampler
    {
        private GPerfBatteryMetric m_BatteryMetric = null;
        public BatterySampler()
        {
#if UNITY_ANDROID
            m_BatteryMetric = new GPerfAndroidBatteryMetric();
#else
            m_BatteryMetric = new GPerfBatteryMetric();
#endif
        }

        public override SamplerType Type => SamplerType.Battery;

        protected override bool Sample(Record record)
        {
            if(m_BatteryMetric!=null)
            {
                record.Data = m_BatteryMetric.GetMetricInfo();
                return true;
            }
            return false;
        }
    }
}
