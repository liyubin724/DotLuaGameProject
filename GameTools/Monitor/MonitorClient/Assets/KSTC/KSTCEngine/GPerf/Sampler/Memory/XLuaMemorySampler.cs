using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSTCEngine.GPerf.Sampler
{
    public class XLuaMemoryRecord:Record
    {
        public float TotalMem { get; set; } = 0.0f;
    }

    public class XLuaMemorySampler : GPerfSampler<XLuaMemoryRecord>
    {
        public XLuaMemorySampler()
        {
            MetricType = SamplerMetricType.XLuaMemory;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 1.0f;
        }

        protected override void OnSample()
        {
            
        }
    }
}
