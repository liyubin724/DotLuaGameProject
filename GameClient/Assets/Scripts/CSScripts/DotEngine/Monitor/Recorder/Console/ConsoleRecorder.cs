using DotEngine.Monitor.Sampler;
using Newtonsoft.Json;
using UnityEngine;

namespace DotEngine.Monitor.Recorder
{
    public class ConsoleRecorder : MonitorRecorder
    {
        public override MonitorRecorderType Type => MonitorRecorderType.Console;

        public override void HandleRecords(MonitorSamplerType type, MonitorRecord[] records)
        {
            if(type!= MonitorSamplerType.Log)
            {
                if (records != null && records.Length > 0)
                {
                    foreach (var record in records)
                    {
                        string json = JsonConvert.SerializeObject(record, Formatting.None);
                        Debug.Log(json);
                    }
                }
            }
        }
    }
}
