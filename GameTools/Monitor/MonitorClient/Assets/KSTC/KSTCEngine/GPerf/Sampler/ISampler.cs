using System;

namespace KSTCEngine.GPerf.Sampler
{
    public enum SamplerMetricType
    {
        None = 0,
        FPS,
        Memory,
        CPU,
        Battery,

        Device,
        App,
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
