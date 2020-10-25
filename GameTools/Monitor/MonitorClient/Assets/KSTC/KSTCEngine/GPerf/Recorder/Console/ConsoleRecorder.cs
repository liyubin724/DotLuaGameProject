﻿using KSTCEngine.GPerf.Sampler;
using Newtonsoft.Json;

namespace KSTCEngine.GPerf.Recorder
{
    public class ConsoleRecorder : GPerfHandleRecorder
    {
        public ConsoleRecorder():base(RecorderType.Console)
        {
        }

        public override void HandleRecord(Record record)
        {
            string json = JsonConvert.SerializeObject(record, Formatting.Indented);
            UnityEngine.Debug.Log($"{GPerfUtil.LOG_NAME}::ConsoleRecorder->{json}");
        }
    }
}
