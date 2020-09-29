using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEngine.Monitor.Sampler
{
    public class FPSRecord : MonitorRecord
    {
        public int FPS { get; set; }
    }

    public class FPSSampler : MonitorSampler<FPSRecord>
    {
        private List<float> m_DeltaTimes = new List<float>();

        public FPSSampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
        }

        protected override MonitorSamplerType Type => MonitorSamplerType.FPS;

        protected override bool OnSample(FPSRecord record)
        {
            if(m_DeltaTimes.Count>0)
            {
                record.FPS = Mathf.RoundToInt(1 / m_DeltaTimes.Average());
                
                m_DeltaTimes.Clear();

                return true;
            }
            return false;
        }

        protected override void OnUpdate(float deltaTime)
        {
            m_DeltaTimes.Add(deltaTime);
        }
    }
}
