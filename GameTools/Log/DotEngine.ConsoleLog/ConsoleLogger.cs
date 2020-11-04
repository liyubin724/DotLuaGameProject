using DotEngine.Log;
using System;
using System.Drawing;

namespace DotEngine.ConsoleLog
{
    public class ConsoleLogger : ILogger
    {
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
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}",Color.White);
        }

        public void LogInfo(string tag, string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}", Color.White);
        }

        public void LogWarning(string tag, string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}", Color.Yellow);
        }

        public void LogError(string tag, string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}", Color.Red);
        }

        public void LogFatal(string tag, string message)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss:ffff")} [{tag}]:{message}", Color.DarkRed);
        }

        public void Close()
        {
        }
    }
}
