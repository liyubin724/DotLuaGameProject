﻿using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace DotEngine.Core.IO
{
    public static class JSONWriter
    {
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
