using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class FPSRecord : Record
    {
        public int FPS { get; set; } = 0;
    }

    public class FPSSampler : GPerfSampler<FPSRecord>
    {
        private List<float> m_FPSList = new List<float>();

        public FPSSampler() 
        {
            MetricType = SamplerMetricType.FPS;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 1.0f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            m_FPSList.Add(1.0f / deltaTime);
        }

        protected override void OnSample(FPSRecord record)
        {
            record.FPS = Mathf.RoundToInt(m_FPSList.Average());
            m_FPSList.Clear();
        }
    }
}
