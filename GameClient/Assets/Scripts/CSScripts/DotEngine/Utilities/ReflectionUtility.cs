using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEngine.Utilities
{
    public static class ReflectionUtility
    {
        private const BindingFlags allBindingFlags = BindingFlags.Instance | BindingFlags.Static
            | BindingFlags.NonPublic | BindingFlags.Public;
        private const BindingFlags allDeclaredBindingFlags = BindingFlags.Instance | BindingFlags.Static 
            | BindingFlags.NonPublic | BindingFlags.Public 
            | BindingFlags.DeclaredOnly;

        #region Get Type

        public static Type GetType(string name, bool isIgnoreCase = false)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            Func<Type, bool> predicate;
            if(name.IndexOf(',')>0)
            {
                predicate = (type) =>
                {
                    return type.AssemblyQualifiedName.Equals(name, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                };
            }else if(name.IndexOf('.')>0)
            {
                predicate = (type) =>
                {
                    return type.FullName.Equals(name, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                };
            }else
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

        public static bool IsSubclassOfRawGeneric(Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static bool IsAssignableFromRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static Type[] GetAllChildClasses(Type baseType,bool allowAbstract = false)
        {
            var types = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var isSubclass = baseType.IsGenericType
                ? (Func<Type, bool>)
                  ((type) => IsSubclassOfRawGeneric(type,baseType))
                : ((type) => type.IsSubclassOf(baseType));

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!isSubclass(type) || !type.IsVisible || (!allowAbstract && type.IsAbstract))
                    {
                        continue;
                    }

                    types.Add(type);
                }
            }

            return types.ToArray();
        }
        #endregion

        #region Get Field
        public static IEnumerable<FieldInfo> GetAllFields(object target,Func<FieldInfo,bool> predicate)
        {
            return GetAllFields(target.GetType(), predicate);
        }

        public static IEnumerable<FieldInfo> GetAllFields(Type type,Func<FieldInfo,bool> predicate)
        {
            var types = new List<Type>() { type };
            while(types.Last().BaseType !=null)
            {
                types.Add(types.Last().BaseType);
            }

            for(int i =0;i<types.Count;++i)
            {
                var fInfos = types[i].GetFields(allDeclaredBindingFlags).Where(predicate);
                foreach(var fInfo in fInfos)
                {
                    yield return fInfo;
                }
            }
        }

        public static FieldInfo GetField(object target, string fieldName, bool isIgnoreCase = false)
        {
            return GetField(target.GetType(), fieldName, isIgnoreCase);
        }

        public static FieldInfo GetField(Type type, string fieldName, bool isIgnoreCase = false)
        {
            return GetAllFields(type, (fInfo) =>
            {
                return fInfo.Name.Equals(fieldName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }).FirstOrDefault();
        }

        public static FieldInfo GetInstanceField(object target, string fieldName, bool isIgnoreCase = false)
        {
            return GetInstanceField(target.GetType(), fieldName, isIgnoreCase);
        }

        public static FieldInfo GetInstanceField(Type type, string fieldName, bool isIgnoreCase = false)
        {
            return GetAllFields(type, (fInfo) =>
            {
                return !fInfo.IsStatic && fInfo.Name.Equals(fieldName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }).FirstOrDefault();
        }

        public static FieldInfo GetStaticField(Type type, string fieldName, bool isIgnoreCase = false)
        {
            return GetAllFields(type, (fInfo) =>
            {
                return fInfo.IsStatic && fInfo.Name.Equals(fieldName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }).FirstOrDefault();
        }

        #endregion

        #region Get Property
        public static IEnumerable<PropertyInfo> GetAllProperties(object target,Func<PropertyInfo,bool> predicate)
        {
            return GetAllProperties(target.GetType(), predicate);
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(Type type,Func<PropertyInfo,bool> predicate)
        {
            var types = new List<Type>() { type };
            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (int i = 0; i < types.Count; ++i)
            {
                var pInfos = types[i].GetProperties(allDeclaredBindingFlags).Where(predicate);
                foreach (var pInfo in pInfos)
                {
                    yield return pInfo;
                }
            }
        }

        public static PropertyInfo GetProperty(object target, string propertyName, bool isIgnoreCase = false)
        {
            return GetProperty(target.GetType(), propertyName, isIgnoreCase);
        }

        public static PropertyInfo GetProperty(Type type, string propertyName, bool isIgnoreCase = false)
        {
            return GetAllProperties(type, (pInfo) =>
            {
                return pInfo.Name.Equals(propertyName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }).FirstOrDefault();
        }

        public static PropertyInfo GetInstanceProperty(object target, string propertyName, bool isIgnoreCase = false)
        {
            return GetInstanceProperty(target.GetType(), propertyName, isIgnoreCase);
        }

        public static PropertyInfo GetInstanceProperty(Type type, string propertyName, bool isIgnoreCase = false)
        {
            return GetAllProperties(type, (pInfo) =>
            {
                bool isInstance = false;
                if (pInfo.GetMethod != null)
                {
                    isInstance = !pInfo.GetMethod.IsStatic;
                }
                else
                {
                    isInstance = !pInfo.SetMethod.IsStatic;
                }
                return isInstance && pInfo.Name.Equals(propertyName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal); ;
            }).FirstOrDefault();
        }

        public static PropertyInfo GetStaticProperty(Type type, string propertyName, bool isIgnoreCase = false)
        {
            return GetAllProperties(type, (pInfo) =>
            {
                bool isStatic = false;
                if(pInfo.GetMethod!=null)
                {
                    isStatic = pInfo.GetMethod.IsStatic;
                }else
                {
                    isStatic = pInfo.SetMethod.IsStatic;
                }
                return isStatic && pInfo.Name.Equals(propertyName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal); ;
            }).FirstOrDefault();
        }
        #endregion

        #region Get Method
        public static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
        {
            return GetAllMethods(target.GetType(), predicate);
        }

        public static IEnumerable<MethodInfo> GetAllMethods(Type type,Func<MethodInfo,bool> predicate)
        {
            var mInfos = type.GetMethods(allBindingFlags)
                .Where(predicate);
            foreach (var mInfo in mInfos)
            {
                yield return mInfo;
            }
        }

        public static MethodInfo GetMethod(object target, string methodName, bool isIgnoreCase = false)
        {
            return GetMethod(target.GetType(), methodName, isIgnoreCase);
        }

        public static MethodInfo GetMethod(Type type,string methodName,bool isIgnoreCase = false)
        {
            return GetAllMethods(type, (mInfo) =>
            {
                return mInfo.Name.Equals(methodName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }).FirstOrDefault();
        }

        public static MethodInfo GetInstanceMethod(object target, string methodName, bool isIgnoreCase = false)
        {
            return GetInstanceMethod(target.GetType(), methodName, isIgnoreCase);
        }

        public static MethodInfo GetInstanceMethod(Type type, string methodName, bool isIgnoreCase = false)
        {
            return GetAllMethods(type, (mInfo) =>
            {
                return !mInfo.IsStatic && mInfo.Name.Equals(methodName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }).FirstOrDefault();
        }

        public static MethodInfo GetStaticMethod(Type type, string methodName, bool isIgnoreCase = false)
        {
            return GetAllMethods(type, (mInfo) =>
            {
                return mInfo.IsStatic && mInfo.Name.Equals(methodName, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
            }).FirstOrDefault();
        }
        #endregion
    }
}
