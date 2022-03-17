using System;
using System.Collections.Generic;
using SystemObject = System.Object;

namespace DotEngine.Utilities
{
    public static class TypeUtility
    {
        /// <summary>
        /// 扩展方法，可以显示一个更具可读性的类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPrettyName(this Type type)
        {
            if (type == null) return "null";

            if (type == typeof(SystemObject)) return "object";
            if (type == typeof(float)) return "float";
            else if (type == typeof(int)) return "int";
            else if (type == typeof(long)) return "long";
            else if (type == typeof(double)) return "double";
            else if (type == typeof(string)) return "string";
            else if (type == typeof(bool)) return "bool";
            else if (type.IsGenericType)
            {
                string s = "";
                Type genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(List<>)) s = "List";
                else s = type.GetGenericTypeDefinition().ToString();

                Type[] types = type.GetGenericArguments();
                string[] stypes = new string[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    stypes[i] = types[i].GetPrettyName();
                }
                return $"{s}<{string.Join(", ", stypes)}>";
            }
            else if (type.IsArray)
            {
                string rank = "";
                for (int i = 1; i < type.GetArrayRank(); i++)
                {
                    rank += ",";
                }
                Type elementType = type.GetElementType();
                if (!elementType.IsArray)
                {
                    return elementType.GetPrettyName() + "[" + rank + "]";
                }
                else
                {
                    string s = elementType.GetPrettyName();
                    int i = s.IndexOf('[');
                    return s.Substring(0, i) + "[" + rank + "]" + s.Substring(i);
                }
            }
            else
            {
                return type.ToString();
            }
        }
    }
}
