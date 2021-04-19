using Newtonsoft.Json;
using System;
using SystemObject = System.Object;

namespace DotEngine.Utilities
{
    public static class JsonUtility
    {
        public static string ToJsonWithType(SystemObject data)
        {
                return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                });
        }

        public static SystemObject FromJsonWithType(string json)
        {
                return JsonConvert.DeserializeObject(json, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                });
        }

        public static string ToJson<T>(T data)
        {
                return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static T FromJson<T>(string json)
        {
                return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
