using CSObjectWrapEditor;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XLua;

namespace DotEditor.Lua.Gen
{
    public static class XLuaGenConfig
    {
        [GenPath]
        public static string GetGenPath
        {
            get
            {
                return "Assets/Scripts/CSScripts/XLuaGen";
            }
        }

        public static List<Type> GetInnerCSharpCallLuaTypeList
        {
            get
            {
                List<Type> callTypes = new List<Type>();

                callTypes.Add(typeof(Action));
                callTypes.Add(typeof(Action<float, float>));
                callTypes.Add(typeof(Func<string, LuaTable>));

                return callTypes;
            }
        }

        [LuaCallCSharp]
        public static List<Type> GetLuaCallCSharpTypeList
        {
            get
            {
                List<Type> callTypes = new List<Type>();
                GenConfig genConfig = GenConfig.GetConfig(false);
                if (genConfig != null)
                {
                    foreach(var typeFullName in genConfig.callCSharpTypeNames)
                    {
                        Type t = AssemblyUtility.GetTypeByFullName(typeFullName);
                        if (t != null)
                        {
                            callTypes.Add(t);
                        }
                        else
                        {
                            Debug.LogError("Type Not Found.name = "+typeFullName);
                        }
                    }

                    foreach(var generic in genConfig.callCSharpGenericTypeNames)
                    {
                        Type t = GetGenericType(generic);
                        if(t!=null)
                        {
                            callTypes.Add(t);
                        }else
                        {
                            Debug.LogError("Type Not Found");
                        }
                    }

                }
                return callTypes;
            }
        }

        private static Type GetGenericType(string genericTypeName)
        {
            if(string.IsNullOrEmpty(genericTypeName))
            {
                return null;
            }
            string[] types = genericTypeName.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (types == null || types.Length < 2)
            {
                return null;
            }
            string genericType = types[0];
            string[] paramTypes = new string[types.Length - 1];
            Array.Copy(types, 1, paramTypes, 0, paramTypes.Length);

            return AssemblyUtility.GetGenericType(genericType, paramTypes);
        }

        [CSharpCallLua]
        public static List<Type> GetCSharpCallLuaTypeList
        {
            get
            {
                List<Type> callTypes = new List<Type>(GetInnerCSharpCallLuaTypeList);
                GenConfig genConfig = GenConfig.GetConfig(false);
                if (genConfig != null)
                {
                    foreach (var typeFullName in genConfig.callLuaTypeNames)
                    {
                        Type t = AssemblyUtility.GetTypeByFullName(typeFullName);
                        if (t != null)
                        {
                            callTypes.Add(t);
                        }
                        else
                        {
                            Debug.LogError("Type Not Found.type = "+typeFullName);
                        }
                    }

                    foreach (var generic in genConfig.callLuaGenericTypeNames)
                    {
                        Type t = GetGenericType(generic);
                        if (t != null)
                        {
                            callTypes.Add(t);
                        }
                        else
                        {
                            Debug.LogError("Type Not Found.type = "+generic);
                        }
                    }
                }
                return callTypes;
            }
        }

        [GCOptimize]
        public static List<Type> GetGCOptimizeTypeList
        {
            get
            {
                List<Type> callTypes = new List<Type>();
                GenConfig genConfig = GenConfig.GetConfig(false);
                if (genConfig != null)
                {
                    foreach (var typeFullName in genConfig.optimizeTypeNames)
                    {
                        callTypes.Add(AssemblyUtility.GetTypeByFullName(typeFullName));
                    }
                }
                return callTypes;
            }
        }

        [BlackList]

        public static List<List<string>> GetBlackList
        {
            get
            {
                List<List<string>> result = new List<List<string>>();
                GenConfig genConfig = GenConfig.GetConfig(false);
                if(genConfig != null)
                {
                    foreach (var blackStr in genConfig.blackDatas)
                    {
                        List<string> list = new List<string>();
                        list.AddRange(blackStr.Split(new char[] { '@', '$' }, StringSplitOptions.RemoveEmptyEntries));
                        result.Add(list);
                    }
                }

                return result;
            }
        }

        //黑名单
        [BlackList]
        public static List<List<string>> BlackList = new List<List<string>>()  {
                new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
                new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
                new List<string>(){"UnityEngine.WWW", "MovieTexture"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
                new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
                new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
            };

#if UNITY_2018_1_OR_NEWER
        [BlackList]
        public static Func<MemberInfo, bool> MethodFilter = (memberInfo) =>
        {
            if (memberInfo.DeclaringType.IsGenericType && memberInfo.DeclaringType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                if (memberInfo.MemberType == MemberTypes.Constructor)
                {
                    ConstructorInfo constructorInfo = memberInfo as ConstructorInfo;
                    var parameterInfos = constructorInfo.GetParameters();
                    if (parameterInfos.Length > 0)
                    {
                        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(parameterInfos[0].ParameterType))
                        {
                            return true;
                        }
                    }
                }
                else if (memberInfo.MemberType == MemberTypes.Method)
                {
                    var methodInfo = memberInfo as MethodInfo;
                    if (methodInfo.Name == "TryAdd" || methodInfo.Name == "Remove" && methodInfo.GetParameters().Length == 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        };
#endif
    }
}
