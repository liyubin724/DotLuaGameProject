using System;
using UnityEngine;

namespace DotEngine.Log
{
    public class UnityLogger : ILogger
    {
        public UnityLogger()
        { }

        public void Log(LogLevelType levelType, string tag, string message)
        {
            if (levelType == LogLevelType.Debug)
            {
                LogDebug(tag, message);
            }
            else if (levelType == LogLevelType.Info)
            {
                LogInfo(tag, message);
            }
            else if (levelType == LogLevelType.Warning)
            {
                LogWarning(tag, message);
            }
            else if (levelType == LogLevelType.Error)
            {
                LogError(tag, message);
            }
            else if (levelType == LogLevelType.Fatal)
            {
                LogFatal(tag, message);
            }
        }

        public void LogDebug(string tag, string message)
        {
            Debug.Log($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}");
        }

        public void LogInfo(string tag, string message)
        {
            Debug.Log($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}");
        }

        public void LogWarning(string tag, string message)
        {
            Debug.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}");
        }

        public void LogError(string tag, string message)
        {
            Debug.LogError($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}");
        }

        public void LogFatal(string tag, string message)
        {
            Debug.LogError($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}");
        }

        public void Close()
        {
            
        }
    }
}
