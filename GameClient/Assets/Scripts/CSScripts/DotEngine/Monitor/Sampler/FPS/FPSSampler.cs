using DotEngine.PMonitor.Recorder;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEngine.PMonitor.Sampler
{
    public class FPSSampler : ASampler<FPSRecord>
    {
        private List<float> deltaTimeList = new List<float>();

        public FPSSampler(IRecorder recorder) : base(SamplerCategory.FPS, recorder)
        {
        }

        protected override void InnerUpdate(float deltaTime)
        {
            deltaTimeList.Add(deltaTime);
        }

        protected override void DoSample(FPSRecord data)
        {
            data.DeltaTime = deltaTimeList.Sum() / deltaTimeList.Count;
            data.FrameIndex = Time.frameCount;
            data.FPS = Mathf.RoundToInt(1 / data.DeltaTime);

            deltaTimeList.Clear();
        }
    }
}
