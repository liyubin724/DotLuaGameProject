using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public static bool CanCastableTo(this Type from, Type to)
        {
            if (to.IsAssignableFrom(from))
            {
                return true;
            }
            var methods = from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(
                    m => m.ReturnType == to &&
                    (m.Name == "op_Implicit" ||
                        m.Name == "op_Explicit")
                );
            return methods.Count() > 0;
        }

        public static bool IsStaticClass(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

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

        public static bool IsStructOrClassType(this Type type) => IsClassType(type) || IsStructType(type);

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

        public static Type[] GetBaseTypes(this Type type,Type suspendType = null)
        {
            if(type == suspendType)
            {
                return new Type[0];
            }
            suspendType = suspendType ?? typeof(object);

            var types = new List<Type>() { type };
            while (types.Last().BaseType != suspendType)
            {
                types.Add(types.Last().BaseType);
            }
            types.Reverse();

            return types.ToArray();
        }

        public static Type[] GetDerivedTypes(this Type baseType)
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    types.AddRange(
                        assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t)).ToArray()
                        );
                }
                catch (ReflectionTypeLoadException) { }
            }
            return types.ToArray();
        }

    }
}
