using DotEngine.BD.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BD
{
    public static class MenuUtility
    {
        private static Type[] sm_TrackGroupDataTypes = null;
        private static Type[] GetTrackGroupDataTypes()
        {
            if (sm_TrackGroupDataTypes == null)
            {
                sm_TrackGroupDataTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                          from type in assembly.GetTypes()
                                          where !type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(TrackGroupData))
                                          select type).ToArray();
            }
            return sm_TrackGroupDataTypes;
        }

        private static Type[] sm_TrackDataTypes = null;
        private static Type[] GetTrackDataTypes()
        {
            if(sm_TrackDataTypes == null)
            {
                sm_TrackDataTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where !type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(TrackData))
                                     select type).ToArray();
            }
            return sm_TrackDataTypes;
        }

        private static Type[] sm_ActionDataTypes = null;
        private static Type[] GetActionDataTypes()
        {
            if(sm_ActionDataTypes == null)
            {
                sm_ActionDataTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where !type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(ActionData))
                                     select type).ToArray();
            }
            return sm_ActionDataTypes;
        }

        private static Dictionary<Type, List<Type>> sm_AllowedTrackDataTypeDic = new Dictionary<Type, List<Type>>();
        private static Type[] GetAllowedTrackDataTypes(Type trackGroupDataType)
        {
            if(!sm_AllowedTrackDataTypeDic.TryGetValue(trackGroupDataType,out var typeList))
            {
                typeList = new List<Type>();
                sm_AllowedTrackDataTypeDic.Add(trackGroupDataType, typeList);
                var tgdAttrs = trackGroupDataType.GetCustomAttributes(typeof(TrackGroupDataAttribute), false);
                if (tgdAttrs != null && tgdAttrs.Length > 0)
                {
                    TrackGroupDataAttribute tgdAttr = (TrackGroupDataAttribute)tgdAttrs[0];
                    if (tgdAttr != null && tgdAttr.AllowedTrackCategories!=null && tgdAttr.AllowedTrackCategories.Length>0)
                    {
                        foreach(var type in GetTrackDataTypes())
                        {
                            var tdAttrs = type.GetCustomAttributes(typeof(TrackDataAttribute), false);
                            if(tdAttrs!=null && tdAttrs.Length>0)
                            {
                                TrackDataAttribute tdAttr = (TrackDataAttribute)tdAttrs[0];
                                if(Array.IndexOf(tgdAttr.AllowedTrackCategories,tdAttr.Category)>=0)
                                {
                                    typeList.Add(type);
                                }
                            }
                        }
                    }
                }
            }
            return typeList.ToArray();
        }

        private static Dictionary<Type, List<Type>> sm_AllowedActionDataTypeDic = new Dictionary<Type, List<Type>>();
        private static Type[] GetAllowedActionDataTypes(Type trackDataType)
        {
            if(!sm_AllowedActionDataTypeDic.TryGetValue(trackDataType,out var typeList))
            {
                typeList = new List<Type>();
                sm_AllowedActionDataTypeDic.Add(trackDataType, typeList);
                var tdAttrs = trackDataType.GetCustomAttributes(typeof(TrackDataAttribute), false);
                if (tdAttrs != null && tdAttrs.Length > 0)
                {
                    TrackDataAttribute tdAttr = (TrackDataAttribute)tdAttrs[0];
                    if (tdAttr != null && tdAttr.AllowedActionCategories != null && tdAttr.AllowedActionCategories.Length > 0)
                    {
                        foreach (var type in GetActionDataTypes())
                        {
                            var aAttrs = type.GetCustomAttributes(typeof(ActionDataAttribute), false);
                            if (aAttrs != null && aAttrs.Length > 0)
                            {
                                ActionDataAttribute aAttr = (ActionDataAttribute)tdAttrs[0];
                                if (aAttr.Categories != null && aAttr.Categories.Length > 0 && aAttr.Categories.Intersect(tdAttr.AllowedActionCategories).Count() > 0)
                                {
                                    typeList.Add(type);
                                }
                            }
                        }
                    }
                }
            }

            return typeList.ToArray();
        }

        public static void ShowCreateTrackGroupMenu(Action<TrackGroupData> createdCallback)
        {
            if(sm_TrackGroupDataTypes == null)
            {
                sm_TrackGroupDataTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                          from type in assembly.GetTypes()
                                          where !type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(TrackGroupData))
                                          select type).ToArray();
            }

            GenericMenu menu = new GenericMenu();
            foreach(var type in sm_TrackGroupDataTypes)
            {
                var attrs = type.GetCustomAttributes(typeof(TrackGroupDataAttribute), false);
                if(attrs !=null && attrs.Length>0)
                {
                    TrackGroupDataAttribute attr = (TrackGroupDataAttribute)attrs[0];
                    if(attr!=null)
                    {
                        menu.AddItem(new GUIContent(attr.Label), false, () =>
                        {
                            TrackGroupData data = Activator.CreateInstance(type) as TrackGroupData;
                            createdCallback?.Invoke(data);
                        });
                    }
                }
            }
            menu.ShowAsContext();
        }

        public static void ShowCreateTrackMenu(Type trackGroupDataType,Action<TrackData> createdCallback)
        {
            Type[] allowedTrackDataTypes = GetAllowedTrackDataTypes(trackGroupDataType);

            GenericMenu menu = new GenericMenu();
            foreach (var type in allowedTrackDataTypes)
            {
                var attrs = type.GetCustomAttributes(typeof(TrackDataAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    TrackDataAttribute attr = (TrackDataAttribute)attrs[0];
                    if (attr != null)
                    {
                        menu.AddItem(new GUIContent(attr.Label), false, () =>
                        {
                            TrackData data = Activator.CreateInstance(type) as TrackData;
                            createdCallback?.Invoke(data);
                        });
                    }
                }
            }
            menu.ShowAsContext();
        }

        public static void ShowCreateActionMenu(Type trackDataType,Action<ActionData> createdCallback)
        {
            Type[] allowedActionDataTypes = GetAllowedActionDataTypes(trackDataType);
            GenericMenu menu = new GenericMenu();
            foreach (var type in allowedActionDataTypes)
            {
                var attrs = type.GetCustomAttributes(typeof(ActionDataAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    ActionDataAttribute attr = (ActionDataAttribute)attrs[0];
                    if (attr != null)
                    {
                        menu.AddItem(new GUIContent(attr.Label), false, () =>
                        {
                            ActionData data = Activator.CreateInstance(type) as ActionData;
                            createdCallback?.Invoke(data);
                        });
                    }
                }
            }
            menu.ShowAsContext();
        }

    }
}
