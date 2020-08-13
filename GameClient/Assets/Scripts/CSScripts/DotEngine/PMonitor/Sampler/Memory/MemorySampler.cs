using DotEngine.PMonitor.Recorder;
using UnityEngine;
using UnityEngine.Profiling;

namespace DotEngine.PMonitor.Sampler
{
    public class MemorySampler : ASampler<MemoryRecord>
    {
        public MemorySampler(IRecorder recorder) : base(SamplerCategory.Memory, recorder)
        {
            SamplingFrameRate = 30;
        }

        protected override void DoSample(MemoryRecord data)
        {
            data.FrameIndex = Time.frameCount;
            data.MaxUsedMemory = Profiler.maxUsedMemory;
            data.UsedHeapSizeLong = Profiler.usedHeapSizeLong;
            data.MonoHeapSizeLong = Profiler.GetMonoHeapSizeLong();
            data.MonoUsedSizeLong = Profiler.GetMonoUsedSizeLong();
            data.TempAllocatorSize = Profiler.GetTempAllocatorSize();
            data.TotalAllocatedMemoryLong = Profiler.GetTotalAllocatedMemoryLong();
            data.TotalReservedMemoryLong = Profiler.GetTotalReservedMemoryLong();
            data.TotalUnusedReservedMemoryLong = Profiler.GetTotalUnusedReservedMemoryLong();

        }

        protected override void Init()
        {
            
        }
    }
}
