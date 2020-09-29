using DotEngine.Lua;
using System;
using XLua;

namespace DotEngine.Monitor.Sampler
{
    public class XLuaMemoryRecord:MonitorRecord
    {
        public float TotalMemory { get; set; } = 0.0f;
    }

    public class XLuaMemorySampler : MonitorSampler<XLuaMemoryRecord>
    {
        public XLuaMemorySampler(Action<MonitorSamplerType, MonitorRecord[]> handleAction) : base(handleAction)
        {
        }

        protected override MonitorSamplerType Type => MonitorSamplerType.XLuaMemory;

        protected override bool OnSample(XLuaMemoryRecord record)
        {
            Facade facade = Facade.GetInstance();
            if (facade != null)
            {
                LuaEnvService service = facade.GetServicer<LuaEnvService>(LuaEnvService.NAME);
                if (service != null && service.IsValid())
                {
                    record.TotalMemory = service.Env.GetTotalMemory();
                    return true;
                }
            }

            return false;
        }
    }
}
