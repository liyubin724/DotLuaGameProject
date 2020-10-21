using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class AppRecord : Record
    {
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Engine { get; set; } = "Unity3D";
        public string EngineVersion { get; set; } = string.Empty;
    }

    public class AppSampler : GPerfSampler<AppRecord>
    {
        public AppSampler()
        {
            MetricType = SamplerMetricType.App;
            FreqType = SamplerFreqType.Start;
        }

        protected override void OnStart()
        {
            record.ID = Application.identifier;
            record.Name = Application.productName;
            record.Version = Application.version;
            record.EngineVersion = Application.unityVersion;
        }

        protected override void OnSample()
        {
        }
    }
}
