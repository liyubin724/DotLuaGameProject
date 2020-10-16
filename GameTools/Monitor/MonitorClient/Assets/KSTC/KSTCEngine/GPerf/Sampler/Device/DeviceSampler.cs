using KSTCEngine.GPerf.Metric;

namespace KSTCEngine.GPerf.Sampler
{
    public class DeviceSampler : GPerfSampler
    {
        private GPerfDeviceMetric m_DeviceMetric = null;
        public DeviceSampler() : base()
        {
            m_DeviceMetric = new GPerfDeviceMetric();
        }

        public override SamplerType Type => SamplerType.Device;

        protected override bool Sample(Record record)
        {
            if (m_DeviceMetric != null)
            {
                record.Data = m_DeviceMetric.GetMetricInfo();
                return true;
            }
            return false;
        }
    }
}
