using System;
using System.Collections.Generic;

namespace DotEngine.Injection
{
    public static class InjectReflection
    {
        private static Dictionary<Type, InjectReflectionTypeInfo> typeInfoDic = new Dictionary<Type, InjectReflectionTypeInfo>();

        internal static InjectReflectionTypeInfo GetTypeInfo(Type type)
        {
            if(typeInfoDic.TryGetValue(type,out var typeInfo))
            {
                return typeInfo;
            }

            RegisterTypeInfo(type);

            return typeInfoDic[type];
        }

        public static void RegisterTypeInfo(Type type,bool isDelayReflect = true)
        {
            if(!typeInfoDic.ContainsKey(type))
            {
                InjectReflectionTypeInfo typeInfo = new InjectReflectionTypeInfo(type);
                if (!isDelayReflect)
                {
                    typeInfo.ReflectMemebers();
                }
                typeInfoDic.Add(type, typeInfo);
            }
        }

        public static void UnregisterTypeInfo(Type type)
        {
            typeInfoDic.Remove(type);
        }

        public static void Clear()
        {
            typeInfoDic.Clear();
        }
    }
}
