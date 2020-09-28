using DotEngine.Monitor.Sampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Monitor.Recorder
{
    public class ConsoleRecorder : MonitorRecorder
    {
        public override MonitorRecorderType Type => MonitorRecorderType.Console;

        public override void HandleRecords(MonitorSamplerType type, MonitorRecord[] records)
        {
            if(type!= MonitorSamplerType.Log)
            {

            }
        }
    }
}
