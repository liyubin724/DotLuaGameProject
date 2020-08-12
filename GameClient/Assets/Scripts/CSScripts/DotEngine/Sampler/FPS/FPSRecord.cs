using System;

namespace DotEngine.Sampler.FPS
{
    public class FPSRecord : RecordData
    {
        public int frameIndex;
        public float deltaTime;
        public int fps;

        public FPSRecord():base(RecordCategory.FPS)
        {
        }
    }
}

