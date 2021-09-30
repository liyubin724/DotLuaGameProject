using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;

namespace DotEngine.Core.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// 扩展方法，用于判断能否从当前类型转换到指定的类型
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static bool IsCastableTo(this Type from, Type to)
        {
            if (to.IsAssignableFrom(from)) return true;
            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == to &&
                    (m.Name == "op_Implicit" ||
                        m.Name == "op_Explicit")
                );
            return methods.Count() > 0;
        }

        /// <summary>
        /// 扩展方法，可以显示一个更具可读性的类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string PrettyName(this Type type)
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
                    stypes[i] = types[i].PrettyName();
                }
                return s + "<" + string.Join(", ", stypes) + ">";
            }
            else if (type.IsArray)
            {
                string rank = "";
                for (int i = 1; i < type.GetArrayRank(); i++)
                {
                    rank += ",";
                }
                Type elementType = type.GetElementType();
                if (!elementType.IsArray) return elementType.PrettyName() + "[" + rank + "]";
                else
                {
                    string s = elementType.PrettyName();
                    int i = s.IndexOf('[');
                    return s.Substring(0, i) + "[" + rank + "]" + s.Substring(i);
                }
            }
            else return type.ToString();
        }

        public static Type GetElementTypeInArrayOrList(this Type type)
        {
            if (IsArrayType(type))
            {
                return type.GetElementType();
            }
            else if (IsListType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return null;
        }

        public static bool IsStructOrClassType(this Type type) => IsClassType(type) || IsStructType(type);

        public static bool IsClassType(this Type type)
        {
            return type.IsClass &&
                type != typeof(string) &&
                !IsDelegateType(type) &&
                !IsArrayOrListType(type);
        }

        public static bool IsStructType(this Type type) => !type.IsEnum && type.IsValueType && !type.IsPrimitive;

        public static bool IsArrayOrListType(this Type type) => IsListType(type) || IsArrayType(type);

        public static bool IsListType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

        public static bool IsArrayType(this Type type) => type.IsArray;

        public static bool IsPrimitiveType(this Type type) => (type.IsValueType && type.IsPrimitive) || type == typeof(string);

        public static bool IsEnumType(this Type type) => type.IsEnum;

        public static bool IsDelegateType(this Type type) => typeof(Delegate).IsAssignableFrom(type);

        public static Type[] GetBaseTypes(this Type type)
        {
            var types = new List<Type>() { type };
            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            types.Reverse();

            return types.ToArray();
        }
    }
}
