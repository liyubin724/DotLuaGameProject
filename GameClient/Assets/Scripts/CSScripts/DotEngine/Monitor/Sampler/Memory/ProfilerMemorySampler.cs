using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace DotEngine.Monitor.Sampler
{
    public class ProfilerMemeoryRecord : MonitorRecord
    {
        public int MaxUsedMemory { get; set; }
        public long UsedHeapSizeLong { get; set; }
        public long MonoHeapSizeLong { get; set; }
        public long MonoUsedSizeLong { get; set; }
        public long TempAllocatorSize { get; set; }
        public long TotalAllocatedMemoryLong { get; set; }
        public long TotalReservedMemoryLong { get; set; }
        public long TotalUnusedReservedMemoryLong { get; set; }
    }

    public class ProfilerMemorySampler : MonitorSampler<ProfilerMemeoryRecord>
    {
        public ProfilerMemorySampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
        }

        protected override MonitorSamplerType Type => MonitorSamplerType.ProfilerMemory;

        protected override bool OnSample(ProfilerMemeoryRecord record)
        {
            record.FrameIndex = Time.frameCount;
            record.MaxUsedMemory = Profiler.maxUsedMemory;
            record.UsedHeapSizeLong = Profiler.usedHeapSizeLong;
            record.MonoHeapSizeLong = Profiler.GetMonoHeapSizeLong();
            record.MonoUsedSizeLong = Profiler.GetMonoUsedSizeLong();
            record.TempAllocatorSize = Profiler.GetTempAllocatorSize();
            record.TotalAllocatedMemoryLong = Profiler.GetTotalAllocatedMemoryLong();
            record.TotalReservedMemoryLong = Profiler.GetTotalReservedMemoryLong();
            record.TotalUnusedReservedMemoryLong = Profiler.GetTotalUnusedReservedMemoryLong();

            return true;
        }
    }
}
