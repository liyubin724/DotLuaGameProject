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
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static T ReadFromText<T>(string text) where T : class
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
