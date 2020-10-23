using System;

namespace KSTCEngine.GPerf.Sampler
{
    public enum SamplerMetricType
    {
        None = 0,
        FPS,
        FrameTime,
        SystemMemory,
        ProfilerMemory,
        CPU,
        Battery,

        Device,
        App,
        Log,
    }
     
    public enum SamplerFreqType
    {
        Start = 0,
        Interval,
        End,
    }

    public interface ISampler
    {
        SamplerMetricType MetricType { get; }
        SamplerFreqType FreqType { get; }
        Action<Record> OnSampleRecord { get; set; }

        float SamplingInterval { get; set; }

        Record GetRecord();

        void DoStart();
        void DoSample();
        void DoUpdate(float deltaTime);
        void DoEnd();
        void DoDispose();
    }
}
