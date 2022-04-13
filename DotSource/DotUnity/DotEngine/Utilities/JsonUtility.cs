using Newtonsoft.Json;
using System;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEngine.Utilities
{
    public static class JsonUtility
    {
        public static string ToJsonWithType(SystemObject data)
        {
            try
            {
                return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                });
            }
            catch(Exception e)
            {
                Debug.Log("JsonUtility::ToJsonWithType->" + e.Message);
            }
            return string.Empty;
        }

        public static SystemObject FromJsonWithType(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                });
            }
            catch (Exception e)
            {
                Debug.Log("JsonUtility::FromJsonWithType->" + e.Message);
            }
            return null;
        }

        public static string ToJson<T>(T data)
        {
            try
            {
                return JsonConvert.SerializeObject(data, Formatting.Indented);
            }
            catch (Exception e)
            {
                Debug.Log("JsonUtility::FromJson->" + e.Message);
            }
            return string.Empty;
        }

        public static T FromJson<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Debug.Log("JsonUtility::FromJson->" + e.Message);
            }
            return default;
        }
    }
}
