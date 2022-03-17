using DotEngine.Core.Exceptions;
using DotEngine.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Core.Utilities
{
    public static class AssemblyUtility
    {
        public static Type GetType(string name, bool isIgnoreCase = false)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            Func<Type, bool> predicate;
            if (name.IndexOf(',') > 0)
            {
                predicate = (type) =>
                {
                    return type.AssemblyQualifiedName.Equals(name, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                };
            }
            else if (name.IndexOf('.') > 0)
            {
                predicate = (type) =>
                {
                    return type.FullName.Equals(name, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                };
            }
            else
            {
                predicate = (type) =>
                {
                    return type.Name.Equals(name, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                };
            }

            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    where predicate(type)
                    select type).FirstOrDefault();
        }

        public static Type[] GetCustomAttributeClassTypes<T>(bool attrInherit = false, bool allowInvisible = false, bool allowAbstract = false) where T : Attribute
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    where Predicate(type)
                    let attrs = type.GetCustomAttributes(typeof(T), attrInherit)
                    where attrs != null && attrs.Length > 0
                    select type).ToArray();

            bool Predicate(Type type)
            {
                if (!type.IsClassType())
                {
                    return false;
                }
                if (!type.IsVisible && !allowInvisible)
                {
                    return false;
                }
                if (!type.IsAbstract && !allowAbstract)
                {
                    return false;
                }
                return true;
            }
        }

        public static Type CreateGenericType(string genericTypeFullName, params string[] paramTypeFullNames)
        {
            if (string.IsNullOrEmpty(genericTypeFullName) || paramTypeFullNames == null || paramTypeFullNames.Length == 0)
            {
                throw new ArgumentNullException();
            }

            Type genericType = GetType(genericTypeFullName);
            if (genericType == null)
            {
                throw new TypeNotFoundException(genericTypeFullName);
            }

            Type[] types = new Type[paramTypeFullNames.Length];
            for (int i = 0; i < paramTypeFullNames.Length; i++)
            {
                string typeStr = paramTypeFullNames[i];
                if (string.IsNullOrEmpty(typeStr))
                {
                    throw new ParamNullException();
                }
                Type t = GetType(paramTypeFullNames[i]);
                if (t == null)
                {
                    throw new TypeNotFoundException(paramTypeFullNames[i]);
                }
                types[i] = t;
            }

            Type result = genericType.MakeGenericType(types);
            return result;
        }
    }
}
