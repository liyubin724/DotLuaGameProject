using DotEngine.BehaviourLine.Action;
using DotEngine.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BehaviourLine
{
    public class ActionMenu
    {
        private static Dictionary<string, Type> actionTypeDic = null;
        public static void ShowMenu(Action<ActionData> callback)
        {
            if(actionTypeDic == null)
            {
                actionTypeDic = new Dictionary<string, Type>();

                Type[] dataTypes = AssemblyUtility.GetDerivedTypes(typeof(ActionData));
                if(dataTypes!=null && dataTypes.Length>0)
                {
                    foreach(var type in dataTypes)
                    {
                        ActionMenuAttribute attr = type.GetCustomAttribute<ActionMenuAttribute>();
                        if (attr == null)
                        {
                            continue;
                        }
                        actionTypeDic.Add($"{attr.Prefix}/{attr.Name}", type);
                    }
                }
            }
            if(actionTypeDic.Count == 0)
            {
                return;
            }

            GenericMenu menu = new GenericMenu();
            if(!string.IsNullOrEmpty(LineSetting.Setting.CopiedActionData))
            {
                ActionData data = (ActionData)JsonConvert.DeserializeObject(LineSetting.Setting.CopiedActionData, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                });
                menu.AddItem(new GUIContent($"Paste({data.GetType().Name})"), false, () =>
                {
                    callback.Invoke(data);
                });
                menu.AddSeparator("");
            }

            foreach(var kvp in actionTypeDic)
            {
                menu.AddItem(new GUIContent(kvp.Key), false, () =>
                {
                    callback.Invoke((ActionData)Activator.CreateInstance(kvp.Value));
                });
            }
            menu.ShowAsContext();
        }
    }
}
