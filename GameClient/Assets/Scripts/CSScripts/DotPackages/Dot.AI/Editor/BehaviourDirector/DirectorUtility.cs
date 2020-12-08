using DotEngine.AI.BD;
using DotEngine.AI.BD.Actions;
using DotEngine.AI.BD.Conditions;
using DotEngine.AI.BD.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AI.BD
{
    public static class DirectorUtility
    {
        private static List<Type> sm_NodeTypes = new List<Type>();
        static DirectorUtility()
        {
            sm_NodeTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                            from type in assembly.GetTypes()
                            where !type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(Node))
                            select type).ToList();
        }

        private static Dictionary<NodeTrackCategory, Type[]> sm_ActionInTrackTypeDic = null;
        private static Type[] GetActionInTrackTypes(NodeTrackCategory category)
        {
            return (from type in sm_NodeTypes
                    where type.IsSubclassOf(typeof(DotEngine.AI.BD.Actions.Action))
                    let attrs = type.GetCustomAttributes(typeof(NodeTrackAttribute), false)
                    where attrs != null && attrs.Length > 0
                    let attr = (NodeTrackAttribute)attrs[0]
                    where attr.Category == category
                    select type).ToArray();
        }

        public static void ShowActionInTrackMenu(NodeTrackCategory category, Action<Action> callback)
        {
            if(sm_ActionInTrackTypeDic == null)
            {
                sm_ActionInTrackTypeDic = new Dictionary<NodeTrackCategory, Type[]>();
                for(int i = (int)NodeTrackCategory.None+1;i<(int)NodeTrackCategory.Max;++i)
                {
                    NodeTrackCategory ntCategory = (NodeTrackCategory)i;
                    sm_ActionInTrackTypeDic.Add(ntCategory, GetActionInTrackTypes(ntCategory));
                }
            }

            GenericMenu menu = new GenericMenu();
            if(sm_ActionInTrackTypeDic.TryGetValue(category, out var types) && types.Length>0)
            {
                foreach(var type in types)
                {
                    var menuAttrs = type.GetCustomAttributes(typeof(NodeMenuAttribute), false);
                    if(menuAttrs!=null && menuAttrs.Length>0)
                    {
                        var menuAttr = (NodeMenuAttribute)menuAttrs[0];
                        string itemName = string.IsNullOrEmpty(menuAttr.Prefix) ? menuAttr.Name : $"{menuAttr.Prefix}/{menuAttr.Name}";
                        menu.AddItem(new GUIContent(itemName), false, () =>
                        {
                            callback?.Invoke((DotEngine.AI.BD.Actions.Action)(DotEngine.AI.BD.Actions.Action)Activator.CreateInstance((Type)type));
                        });
                    }
                }
            }

            menu.ShowAsContext();
        }

        private static Type[] sm_ConditionTypes = null;
        public static void ShowActionConditionMenu(Action<ConditionNode> callback)
        {
            if(sm_ConditionTypes == null)
            {
                sm_ConditionTypes = (from type in sm_NodeTypes
                                     where type.IsSubclassOf(typeof(ConditionNode))
                                     select type).ToArray();
            }

            GenericMenu menu = new GenericMenu();
            if(sm_ConditionTypes.Length>0)
            {
                foreach (var type in sm_ConditionTypes)
                {
                    var menuAttrs = type.GetCustomAttributes(typeof(NodeMenuAttribute), false);
                    if (menuAttrs != null && menuAttrs.Length > 0)
                    {
                        var menuAttr = (NodeMenuAttribute)menuAttrs[0];
                        string itemName = string.IsNullOrEmpty(menuAttr.Prefix) ? menuAttr.Name : $"{menuAttr.Prefix}/{menuAttr.Name}";
                        menu.AddItem(new GUIContent(itemName), false, () =>
                        {
                            callback?.Invoke((ConditionNode)Activator.CreateInstance(type));
                        });
                    }
                }
            }
            menu.ShowAsContext();
        }
    }
}
