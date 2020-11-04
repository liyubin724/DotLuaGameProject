using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class FPSRecord : Record
    {
        public float DeltaTimeInMS { get; set; } = 0f;
        public int FPS { get; set; } = 0;
    }

    public class FPSSampler : GPerfSampler<FPSRecord>
    {
        private int m_FrameCount = 0;
        private float m_DeltaTime = 0.0f;
        public FPSSampler() 
        {
            MetricType = SamplerMetricType.FPS;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 1.0f;
        }

        protected override void OnUpdate(float deltaTime)
        {
            m_DeltaTime += deltaTime;
            m_FrameCount++;
        }

        protected override void OnSample()
        {
            record.FPS = Mathf.RoundToInt(m_FrameCount / SamplingInterval);
            record.DeltaTimeInMS = m_DeltaTime * 1000 / m_FrameCount;

            m_FrameCount = 0;
            m_DeltaTime = 0.0f;
        }
    }
}
