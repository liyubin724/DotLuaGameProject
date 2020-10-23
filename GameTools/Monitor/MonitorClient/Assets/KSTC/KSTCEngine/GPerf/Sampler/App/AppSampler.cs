using UnityEngine;

namespace KSTCEngine.GPerf.Sampler
{
    public class AppRecord : Record
    {
        public string Identifier { get; set; } = string.Empty;
        public string InstallName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
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
            record.Identifier = Application.identifier;
            record.InstallName = Application.installerName;
            record.ProductName = Application.productName;
            record.Version = Application.version;
            record.EngineVersion = Application.unityVersion;
        }

        protected override void OnSample()
        {
        }
    }
}
