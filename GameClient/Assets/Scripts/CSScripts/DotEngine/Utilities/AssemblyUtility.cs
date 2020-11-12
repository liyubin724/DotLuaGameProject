using DotEngine.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEngine.Utilities
{
    public static class AssemblyUtility
    {
        public static Type GetType(string typeName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.Name == typeName)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        public static Type GetTypeByFullName(string typeFullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach(var type in types)
                {
                    if(type.FullName == typeFullName)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        public static Type GetGenericType(string genericTypeFullName,params string[] paramTypeFullNames)
        {
            if(string.IsNullOrEmpty(genericTypeFullName) || paramTypeFullNames == null || paramTypeFullNames.Length ==0)
            {
                LogUtil.Error(typeof(AssemblyUtility).Name, "AssemblyUtil::GetGenericType->Arg is Null");
                return null;
            }

            Type genericType = GetTypeByFullName(genericTypeFullName);
            if(genericType == null)
            {
                LogUtil.Error(typeof(AssemblyUtility).Name, $"AssemblyUtil::GetGenericType->Type Not Found.Type = {genericTypeFullName}");
                return null;
            }

            Type[] types = new Type[paramTypeFullNames.Length];
            for(int i =0;i<paramTypeFullNames.Length;i++)
            {
                string typeStr = paramTypeFullNames[i];
                if (string.IsNullOrEmpty(typeStr))
                {
                    LogUtil.Error(typeof(AssemblyUtility).Name, "AssemblyUtil::GetGenericType->Param Type Is NUll");
                    return null;
                }
                Type t = GetTypeByFullName(paramTypeFullNames[i]);
                if(t == null)
                {
                    LogUtil.Error(typeof(AssemblyUtility).Name, $"AssemblyUtil::GetGenericType->Param Type Not Found.Type = {paramTypeFullNames[i]}");
                    return null;
                }
                types[i] = t;
            }

            Type result = genericType.MakeGenericType(types);
            return result;
        }

        /// <summary>
        /// 查找所有继承自指定类型的子类，如果子类是abstract将会被忽略
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static Type[] GetDerivedTypes(Type baseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray());
                }
                catch (ReflectionTypeLoadException) { }
            }
            return types.ToArray();
        }
    }
}
