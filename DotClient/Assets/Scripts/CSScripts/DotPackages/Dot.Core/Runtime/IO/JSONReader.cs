using Newtonsoft.Json;
using System.IO;

namespace DotEngine.Core.IO
{
    public static class JSONReader
    {
        public static T ReadFromFile<T>(string filePath) where T : class
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }
            string content = File.ReadAllText(filePath);
            T result = JsonConvert.DeserializeObject<T>(content);
            
            if(typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)result).DoDeserialize();
            }

            return result;
        }

        public static T ReadFromText<T>(string text) where T : class
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            
            T result = JsonConvert.DeserializeObject<T>(text);
            if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
            {
                ((ISerialization)result).DoDeserialize();
            }

            return result;
        }
    }
}
