using System;
using UnityEngine;

namespace DotEngine.Sampler.Log
{
    public class LogRecord : RecordData
    {
        public LogType type;
        public string message;
        public string stackTrace;

        protected LogRecord() : base(RecordCategory.Log)
        {
        }
    }
}