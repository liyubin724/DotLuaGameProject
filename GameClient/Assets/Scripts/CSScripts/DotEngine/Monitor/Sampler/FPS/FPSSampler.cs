using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEngine.Monitor.Sampler
{
    public class FPSRecord : MonitorRecord
    {
        public int FrameIndex { get; set; }
        public int FPS { get; set; }
    }

    public class FPSSampler : MonitorSampler<FPSRecord>
    {
        private List<float> m_DeltaTimes = new List<float>();

        public FPSSampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
        }

        protected override MonitorSamplerType Type => MonitorSamplerType.FPS;

        protected override void OnSample(FPSRecord record)
        {
            record.FrameIndex = Time.frameCount;
            record.FPS = Mathf.RoundToInt(1 / m_DeltaTimes.Sum()/m_DeltaTimes.Count);

            m_DeltaTimes.Clear();
        }

        protected override void OnUpdate(float deltaTime)
        {
            m_DeltaTimes.Add(deltaTime);
        }
    }
}
