using UnityEngine.Profiling;

namespace KSTCEngine.GPerf.Sampler
{
    public class ProfilerMemoryRecord : Record
    {
        public long MonoHeapSizeInKB { get; set; } = 0L;
        public long MonoUsedSizeInKB { get; set; } = 0L;
        public long TempAllocatorSizeInKB { get; set; } = 0L;
        public long TotalAllocatorSizeInKB { get; set; } = 0L;
        public long TotalReservedSizeInKB{get;set;} = 0L;
        public long TotalUnusedReservedSizeInKB { get; set; } = 0L;
        public long AllocatedForGraphicsDriverInKB { get; set; } = 0L;
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
            record.MonoHeapSizeInKB = Profiler.GetMonoHeapSizeLong() / GPerfUtil.BYTE_TO_MB_SIZE;
            record.MonoUsedSizeInKB = Profiler.GetMonoUsedSizeLong() / GPerfUtil.BYTE_TO_MB_SIZE;
            record.TempAllocatorSizeInKB = Profiler.GetTempAllocatorSize() / GPerfUtil.BYTE_TO_MB_SIZE;
            record.TotalAllocatorSizeInKB = Profiler.GetTotalAllocatedMemoryLong() / GPerfUtil.BYTE_TO_MB_SIZE;
            record.TotalReservedSizeInKB = Profiler.GetTotalReservedMemoryLong() / GPerfUtil.BYTE_TO_MB_SIZE;
            record.TotalUnusedReservedSizeInKB = Profiler.GetTotalUnusedReservedMemoryLong() / GPerfUtil.BYTE_TO_MB_SIZE;
            record.AllocatedForGraphicsDriverInKB = Profiler.GetAllocatedMemoryForGraphicsDriver() / GPerfUtil.BYTE_TO_MB_SIZE;
        }
    }
}
