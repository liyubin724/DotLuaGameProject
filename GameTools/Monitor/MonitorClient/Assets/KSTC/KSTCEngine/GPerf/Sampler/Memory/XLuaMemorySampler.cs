#if GPERF_XLUA
using XLua;
#endif

namespace KSTCEngine.GPerf.Sampler
{
    public class XLuaMemoryRecord:Record
    {
        public float TotalMem { get; set; } = 0.0f;
    }

    public class XLuaMemorySampler : GPerfSampler<XLuaMemoryRecord>
    {
        public XLuaMemorySampler()
        {
            MetricType = SamplerMetricType.XLuaMemory;
            FreqType = SamplerFreqType.Interval;
        }

        protected override void OnSample()
        {
#if GPERF_XLUA
            LuaEnv luaEnv = GPerfMonitor.GetInstance().XLuaEnv;
            if(luaEnv!=null)
            {
                record.TotalMem = luaEnv.GetTotalMemory();
            }
#endif
        }
    }
}
