using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class FPSSampler : GPerfSampler
    {
        private List<float> m_FPSList = new List<float>();

        public FPSSampler() : base()
        {
        }

        public override SamplerType Type => SamplerType.FPS;

        protected override bool Sample(Record record)
        {
            if (m_FPSList.Count > 0)
            {
                record.Data = $"FPS:{Mathf.RoundToInt(m_FPSList.Average())}";
                m_FPSList.Clear();
                return true;
            }
            return false;
        }

        public override void DoUpdate(float deltaTime)
        {
            if(m_FPSList.Count>=100)
            {
                m_FPSList.Clear();
            }
            m_FPSList.Add(1.0f/deltaTime);
        }
    }
}
