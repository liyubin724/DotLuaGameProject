using System;
using UnityEngine;

namespace DotEngine.Log
{
    public class UnityLogger : Logger
    {
        public UnityLogger()
        { }

        public override void LogDebug(string tag, string message)
        {
            Debug.Log($"[{tag}]:{message}");
        }

        public override void LogInfo(string tag, string message)
        {
            Debug.Log($"[{tag}]:{message}");
        }

        public override void LogWarning(string tag, string message)
        {
            Debug.LogWarning($"[{tag}]:{message}");
        }

        public override void LogError(string tag, string message)
        {
            Debug.LogError($"[{tag}]:{message}");
        }

        public override void LogFatal(string tag, string message)
        {
            Debug.LogError($"[{tag}]:{message}");
        }

        public override void Close()
        {
        }
    }
}
