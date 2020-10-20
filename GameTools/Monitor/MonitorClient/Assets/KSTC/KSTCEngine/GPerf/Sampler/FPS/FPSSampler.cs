using System;
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
        public override SamplerType SamplerType => SamplerType.FPS;

        private List<float> m_FPSList = new List<float>();

        public FPSSampler() : base()
        {
        }


        public override void DoUpdate(float deltaTime)
        {
            if(m_FPSList.Count>=100)
            {
                m_FPSList.Clear();
            }
            m_FPSList.Add(1.0f/deltaTime);
        }

        protected override void Sampling(FPSRecord record)
        {
            record.FPS = Mathf.RoundToInt(m_FPSList.Average());

            m_FPSList.Clear();
        }
    }
}
