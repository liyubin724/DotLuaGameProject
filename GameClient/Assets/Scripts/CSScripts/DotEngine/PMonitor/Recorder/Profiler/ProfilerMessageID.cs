using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.PMonitor.Recorder
{
    public static class C2S_ProfilerMessageID
    {
        public static int OPEN_SAMPLER_REQUEST = 1;
        public static int CLOSE_SAMPLER_REQUEST = 2;
    }

    public static class S2C_ProfilerMessageID
    {
        public static int OPEN_SAMPLER_RESPONSE = 1001;
        public static int CLOSE_SAMPLER_RESPONSE = 1002;
    }
}
