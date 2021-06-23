#if PLATFORM_UNITY

using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Log
{
    public class UnityConsoleAppender : ALogAppender
    {
        public static readonly string NAME = "UnityConsole";
        private Dictionary<LogLevel, string> colorDic = new Dictionary<LogLevel, string>()
        {
            {LogLevel.Trace,"#DCDCDC" },
            {LogLevel.Debug,"#696969" },
            {LogLevel.Info,"#FFFFF0" },
            {LogLevel.Warning,"#FFD700	" },
            {LogLevel.Error,"#FF0000" },
            {LogLevel.Fatal,"#800000" },
        };

        public UnityConsoleAppender() : this(LogLevelConst.All)
        {
        }

        public UnityConsoleAppender(LogLevel validLevel) : base(NAME, validLevel)
        {
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            if (colorDic.TryGetValue(level, out string color))
            {
                Debug.Log($"<color={color}>{message}</color>");
            }
            else
            {
                Debug.Log(message);
            }
        }
    }
}

#endif