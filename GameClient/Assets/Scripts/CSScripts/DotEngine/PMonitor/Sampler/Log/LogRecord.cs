using UnityEngine;

namespace DotEngine.PMonitor.Sampler
{
    public class LogRecord : Record
    {
        public LogType Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public override void OnNew()
        {
            Category = SamplerCategory.Log;
        }
    }
}
