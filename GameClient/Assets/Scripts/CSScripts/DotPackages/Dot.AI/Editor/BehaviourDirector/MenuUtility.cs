using DotEngine.BD.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

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

        private static Dictionary<Type, List<Type>> sm_TrackGroupDataAllowedTrackDataTypeDic = null;
        public static Type[] GetTrackGroupDataAllowedTrackDataTypes(Type trackGroupDataType)
        {
            if(sm_TrackGroupDataAllowedTrackDataTypeDic == null)
            {
                sm_TrackGroupDataAllowedTrackDataTypeDic = new Dictionary<Type, List<Type>>();
            }
            
            if(sm_TrackGroupDataAllowedTrackDataTypeDic.TryGetValue(trackGroupDataType,out var typeList))
            {
                return typeList.ToArray();
            }else
            {

            }
            return new Type[0];
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

        private static Type[] sm_TrackTypes = null;

        public static void ShowCreateTrackMenu(TrackGroupData groupData)
        {

        }

    }
}
