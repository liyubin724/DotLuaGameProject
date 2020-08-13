namespace DotEngine.PMonitor.Sampler
{
    public class MemoryRecord : Record
    {
        public int FrameIndex { get; set; }
        public int MaxUsedMemory { get; set; }
        public long UsedHeapSizeLong { get; set; }
        public long MonoHeapSizeLong { get; set; }
        public long MonoUsedSizeLong { get; set; }
        public long TempAllocatorSize { get; set; }
        public long TotalAllocatedMemoryLong { get; set; }
        public long TotalReservedMemoryLong { get; set; }
        public long TotalUnusedReservedMemoryLong { get; set; }

        public override void OnNew()
        {
            Category = SamplerCategory.Memory;
        }
    }
}
