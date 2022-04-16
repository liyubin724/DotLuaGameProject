using DotEngine.Core.Serialization;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace DotEngine.Serialization
{
    public static class JSONSerializer
    {
        public static T ReadFromFile<T>(string filePath, JSONFormatSetting setting = null) where T : class
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }
            string content = File.ReadAllText(filePath);
            T result = JsonConvert.DeserializeObject<T>(content, setting ?? JSONFormatSetting.Default);

            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)result).DoDeserialize();
            }

            return result;
        }

        public static T ReadFromText<T>(string text, JSONFormatSetting setting = null) where T : class
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            T result = JsonConvert.DeserializeObject<T>(text, setting ?? JSONFormatSetting.Default);
            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)result).DoDeserialize();
            }

            return result;
        }

        public static void WriteToFile<T>(T data, string filePath, JSONFormatSetting setting = null) where T : class
        {
            if (string.IsNullOrEmpty(filePath) || data == null)
            {
                Debug.LogError("the path or the data is null");
                return;
            }

            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)data).DoSerialize();
            }

            string jsonContent = JsonConvert.SerializeObject(data, setting ?? JSONFormatSetting.Default);
            File.WriteAllText(filePath, jsonContent);
        }

        public static string WriteToText<T>(T data, JSONFormatSetting setting = null) where T : class
        {
            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)data).DoSerialize();
            }

            return JsonConvert.SerializeObject(data, setting ?? JSONFormatSetting.Default);
        }
    }
}
