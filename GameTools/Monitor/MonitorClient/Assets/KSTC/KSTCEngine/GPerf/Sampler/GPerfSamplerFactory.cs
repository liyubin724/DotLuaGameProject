namespace KSTCEngine.GPerf.Sampler
{
    public class GPerfSamplerFactory
    {
        public static ISampler GetSampler(SamplerMetricType metricType)
        {
            ISampler sampler;
            switch (metricType)
            {
                case SamplerMetricType.FPS:
                    sampler = new FPSSampler();
                    break;
                case SamplerMetricType.SystemMemory:
                    sampler = new SystemMemorySampler();
                    break;
                case SamplerMetricType.ProfilerMemory:
                    sampler = new ProfilerMemorySampler();
                    break;
                case SamplerMetricType.XLuaMemory:
                    sampler = new XLuaMemorySampler();
                    break;
                case SamplerMetricType.Device:
                    sampler = new DeviceSampler();
                    break;
                case SamplerMetricType.App:
                    sampler = new AppSampler();
                    break;
                case SamplerMetricType.Battery:
                    sampler = new BatterySampler();
                    break;
                case SamplerMetricType.CPU:
                    sampler = new CPUSampler();
                    break;
                case SamplerMetricType.FrameTime:
                    sampler = new FrameTimeSampler();
                    break;
                case SamplerMetricType.Log:
                    sampler = new LogSampler();
                    break;
                default:
                    sampler = null;
                    break;
            }

            return sampler;
        }
    }
}
