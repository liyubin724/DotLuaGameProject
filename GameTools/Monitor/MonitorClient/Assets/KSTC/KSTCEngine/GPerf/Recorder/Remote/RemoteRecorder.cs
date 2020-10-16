using KSTCEngine.GPerf.Sampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSTCEngine.GPerf.Recorder
{
    public class RemoteRecorder : GPerfRecorder
    {
        public override RecorderType Type => RecorderType.Remote;

        public override void HandleRecords(Record[] records)
        {
            
        }
    }
}
