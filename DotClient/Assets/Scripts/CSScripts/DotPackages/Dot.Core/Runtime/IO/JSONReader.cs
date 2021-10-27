using Newtonsoft.Json;
using System.IO;

namespace DotEngine.Core.IO
{
    public class JSONReaderSetting
    {
        public static JSONReaderSetting Default = new JSONReaderSetting();

        public bool IsUsedClassType { get; set; } = false;

        public static implicit operator JsonSerializerSettings(JSONReaderSetting setting)
        {
            JsonSerializerSettings jsSetting = new JsonSerializerSettings();
            if (setting.IsUsedClassType)
            {
                jsSetting.TypeNameHandling = TypeNameHandling.Auto;
                jsSetting.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            }
            return jsSetting;
        }
    }

    public static class JSONReader
    {
        public static T ReadFromFile<T>(string filePath, JSONReaderSetting setting = null) where T : class
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }
            string content = File.ReadAllText(filePath);
            T result = JsonConvert.DeserializeObject<T>(content, setting ?? JSONReaderSetting.Default);

            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)result).DoDeserialize();
            }

            return result;
        }

        public static T ReadFromText<T>(string text, JSONReaderSetting setting = null) where T : class
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            T result = JsonConvert.DeserializeObject<T>(text, setting ?? JSONReaderSetting.Default);
            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)result).DoDeserialize();
            }

            return result;
        }
    }
}
