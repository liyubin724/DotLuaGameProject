using DotEngine.PMonitor.Sampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.PMonitor.Recorder
{
    public class ProfilerRecorder : ARecorder
    {
        public ProfilerRecorder() : base(RecorderCategory.Profiler)
        {

        }

        public override void Init()
        {
            
        }

        public override void Dispose()
        {
            
        }


        public override void HandleRecord(SamplerCategory category, Record[] records)
        {
            
        }
    }
}
