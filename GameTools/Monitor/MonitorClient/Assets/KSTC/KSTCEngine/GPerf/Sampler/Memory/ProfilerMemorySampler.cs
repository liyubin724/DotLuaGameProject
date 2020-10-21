using UnityEngine.Profiling;

namespace KSTCEngine.GPerf.Sampler
{
    public class ProfilerMemoryRecord : Record
    {
        public long MonoHeapSize { get; set; } = 0L;
        public long MonoUsedSize { get; set; } = 0L;
        public long TempAllocatorSize { get; set; } = 0L;
        public long TotalAllocatorSize { get; set; } = 0L;
        public long TotalReservedMemorySize{get;set;} = 0L;
        public long TotalUnusedReservedMemorySize { get; set; } = 0L;
        public long AllocatedMemoryForGraphicsDriver { get; set; } = 0L;
    }

    public class ProfilerMemorySampler : GPerfSampler<ProfilerMemoryRecord>
    {
        public ProfilerMemorySampler()
        {
            MetricType = SamplerMetricType.ProfilerMemory;
            FreqType = SamplerFreqType.Interval;
            SamplingInterval = 1.0f;
        }

        protected override void OnSample()
        {
            record.MonoHeapSize = Profiler.GetMonoHeapSizeLong();
            record.MonoUsedSize = Profiler.GetMonoUsedSizeLong();
            record.TempAllocatorSize = Profiler.GetTempAllocatorSize();
            record.TotalAllocatorSize = Profiler.GetTotalAllocatedMemoryLong();
            record.TotalReservedMemorySize = Profiler.GetTotalReservedMemoryLong();
            record.TotalUnusedReservedMemorySize = Profiler.GetTotalUnusedReservedMemoryLong();
            record.AllocatedMemoryForGraphicsDriver = Profiler.GetAllocatedMemoryForGraphicsDriver();
        }
    }
}
