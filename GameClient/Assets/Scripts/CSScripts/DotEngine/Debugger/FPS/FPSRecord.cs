using Newtonsoft.Json;
using System;

namespace DotEngine.Debugger.FPS
{
    public class FPSRecord
    {
        public DateTime time;
        public int frameIndex;
        public float deltaTime;
        public int fps;

        public static string ToJson(FPSRecord record)
        {
            return JsonConvert.SerializeObject(record, Formatting.None);
        }

        public static FPSRecord FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FPSRecord>(json);
        }

        public override string ToString()
        {
            return $"[time:{time},frameIndex:{frameIndex},deltaTime:{deltaTime},fps:{fps}]";
        }
    }
}

