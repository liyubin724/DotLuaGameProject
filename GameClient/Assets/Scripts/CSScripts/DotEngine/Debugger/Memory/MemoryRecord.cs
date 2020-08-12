using Newtonsoft.Json;
using System;

namespace DotEngine.Debugger.Memory
{
    public class MemoryRecord
    {
        public DateTime time;
        public int frameIndex;
        public float luaUsed;
        
        public static string ToJson(MemoryRecord record)
        {
            return JsonConvert.SerializeObject(record, Formatting.None);
        }

        public static MemoryRecord FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MemoryRecord>(json);
        }

        public override string ToString()
        {
            return $"[time:{time},frameIndex:{frameIndex},luaUsed:{luaUsed}]";
        }
    }
}
