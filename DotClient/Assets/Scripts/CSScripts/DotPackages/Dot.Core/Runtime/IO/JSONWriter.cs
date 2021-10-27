using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace DotEngine.Core.IO
{
    public class JSONWriterSetting
    {
        public static JSONWriterSetting Default = new JSONWriterSetting();

        public bool IsIndent { get; set; } = true;
        public bool IsUsedClassType { get; set; } = false;
        public bool IsUseDefaultValue { get; set; } = false;

        public static implicit operator JsonSerializerSettings(JSONWriterSetting setting)
        {
            JsonSerializerSettings jsSetting = new JsonSerializerSettings();
            if (setting.IsIndent)
            {
                jsSetting.Formatting = Formatting.Indented;
            }
            else
            {
                jsSetting.Formatting = Formatting.None;
            }
            if (setting.IsUsedClassType)
            {
                jsSetting.TypeNameHandling = TypeNameHandling.Auto;
                jsSetting.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            }
            if (setting.IsUseDefaultValue)
            {
                jsSetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            }
            else
            {
                jsSetting.DefaultValueHandling = DefaultValueHandling.Include;
            }
            return jsSetting;
        }
    }

    public static class JSONWriter
    {
        public static void WriteToFile<T>(T data, string filePath, JSONWriterSetting setting = null) where T : class
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

            string jsonContent = JsonConvert.SerializeObject(data, setting ?? JSONWriterSetting.Default);
            File.WriteAllText(filePath, jsonContent);
        }

        public static string WriteToText<T>(T data, JSONWriterSetting setting = null) where T : class
        {
            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)data).DoSerialize();
            }

            return JsonConvert.SerializeObject(data, setting ?? JSONWriterSetting.Default);
        }
    }
}
