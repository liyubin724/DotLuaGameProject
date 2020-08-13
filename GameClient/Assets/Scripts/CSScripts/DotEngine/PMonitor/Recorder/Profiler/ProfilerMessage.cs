using DotEngine.PMonitor.Sampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.PMonitor.Recorder
{
    public class C2S_OpenSamplerRequest
    {
        public SamplerCategory category;
    }

    public class S2C_OpenSamplerResponse
    {
        public bool result;
    }

    public class C2S_CloseSamplerRequest
    {
        public SamplerCategory category;
    }

    public class S2C_CloseSamplerResponse
    {
        public bool result;
    }


}
