using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace DotEngine.Core.IO
{
    public static class JSONWriter
    {
        public static void WriteToFile<T>(T data,string filePath) where T:class
        {
            if (string.IsNullOrEmpty(filePath) || data == null)
            {
                Debug.LogError("the path or the data is null");
                return;
            }

            string jsonContent = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonContent);
        }

        public static string WriteToText<T>(T data) where T:class
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
