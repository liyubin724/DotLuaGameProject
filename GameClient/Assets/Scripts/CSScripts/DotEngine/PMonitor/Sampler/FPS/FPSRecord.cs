namespace DotEngine.PMonitor.Sampler
{
    public class FPSRecord : Record
    {
        public int FPS { get; set; }
        public float DeltaTime { get; set; }
        public int FrameIndex { get; set; }

        public override void OnNew()
        {
            Category = SamplerCategory.FPS;
        }
    }
}
