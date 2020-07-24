using DotEngine.BehaviourLine.Action;
using DotEngine.BehaviourLine.Line;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace DotEditor.BehaviourLine
{
    public static class LineUtil
    {
        public static ActionData Copy(this ActionData fromAction)
        {
            if (fromAction == null)
            {
                return null;
            }

            Type actionType = fromAction.GetType();
            ActionData data = (ActionData)Activator.CreateInstance(actionType);

            FieldInfo[] fields = actionType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                object value = field.GetValue(fromAction);
                field.SetValue(data, value);
            }

            return data;
        }

        public static void SaveToJsonFile(TimelineData data,string filePath)
        {
            data.Tracks.ForEach((track) =>
            {
                track.Actions.Sort((action1, action2) =>
                {
                    return action1.FireTime.CompareTo(action2.FireTime);
                });
            });

            string jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
            });
            File.WriteAllText(filePath, jsonStr);
        }

        public static TimelineData ReadFromJsonFile(string filePath)
        {
            string jsonStr = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<TimelineData>(jsonStr, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
            });
        }
    }
}
