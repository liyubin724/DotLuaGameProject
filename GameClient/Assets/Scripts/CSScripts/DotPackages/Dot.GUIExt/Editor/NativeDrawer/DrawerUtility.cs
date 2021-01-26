using DotEngine.GUIExt.NativeDrawer;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public static class DrawerUtility
    {
        private static Dictionary<Type, Type> defaultTypeDrawerDic = null;

        private static void LoadTypeDrawers()
        {
            defaultTypeDrawerDic = new Dictionary<Type, Type>();
            Type[] types = AssemblyUtility.GetDerivedTypes(typeof(TypeDrawer));
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(CustomTypeDrawerAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    CustomTypeDrawerAttribute attr = attrs[0] as CustomTypeDrawerAttribute;
                    defaultTypeDrawerDic.Add(attr.TargetType, type);
                }
            }
        }

        public static TypeDrawer GetTypeDrawerInstance(Type type)
        {
            if (defaultTypeDrawerDic == null)
            {
                LoadTypeDrawers();
            }

            Type targetType = type.IsEnum ? typeof(Enum) : type;
            if (defaultTypeDrawerDic.TryGetValue(targetType, out var drawerType) && drawerType != null)
            {
                return (TypeDrawer)Activator.CreateInstance(drawerType);
            }
            return null;
        }

        private static Dictionary<Type, Type> attrTypeDrawerDic = null;
        private static void LoadAttrDrawers()
        {
            attrTypeDrawerDic = new Dictionary<Type, Type>();
            Type[] types = AssemblyUtility.GetDerivedTypes(typeof(IAttrDrawer));
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(CustomAttrDrawerAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    CustomAttrDrawerAttribute attr = attrs[0] as CustomAttrDrawerAttribute;
                    attrTypeDrawerDic.Add(attr.AttrType, type);
                }
            }
        }

        public static IAttrDrawer GetAttrDrawerInstance(DrawerAttribute drawerAttr)
        {
            if (attrTypeDrawerDic == null)
            {
                LoadAttrDrawers();
            }

            if (attrTypeDrawerDic.TryGetValue(drawerAttr.GetType(), out var drawerType) && drawerType != null)
            {
                IAttrDrawer drawer = (IAttrDrawer)Activator.CreateInstance(drawerType);
                drawer.Attr = drawerAttr;
                return drawer;
            }
            return null;
        }

        public static bool IsTypeSupported(Type type)
        {
            return TypeUtility.IsPrimitiveType(type) ||
                TypeUtility.IsStructOrClassType(type) ||
                TypeUtility.IsArrayOrListType(type) ||
                TypeUtility.IsEnumType(type);
        }

        public static SystemObject GetTypeInstance(Type type)
        {
            if (type.IsArray)
            {
                return Array.CreateInstance(TypeUtility.GetElementTypeInArrayOrList(type), 0);
            }
            else if (type == typeof(string))
            {
                return string.Empty;
            }
            else if (typeof(UnityObject).IsAssignableFrom(type))
            {
                return null;
            }
            else
            {
                object instance;
                try
                {
                    instance = Activator.CreateInstance(type);
                }
                catch
                {
                    instance = null;
                }

                return instance;
            }
        }

        public static Type[] GetAllBaseTypes(Type type)
        {
            if ((type.IsValueType && type.IsPrimitive) || type == typeof(string) || type.IsArray || type.IsEnum || typeof(List<>).IsAssignableFrom(type))
            {
                return new Type[0];
            }

            Type[] types = type.GetBaseTypes();
            if (types != null && types.Length > 0)
            {
                Type blockType;
                if (type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    blockType = typeof(MonoBehaviour);
                }
                else if (type.IsSubclassOf(typeof(ScriptableObject)))
                {
                    blockType = typeof(ScriptableObject);
                }
                else if (type.IsSubclassOf(typeof(UnityObject)))
                {
                    blockType = typeof(UnityObject);
                }
                else
                {
                    blockType = typeof(SystemObject);
                }

                for (int i = 0; i < types.Length; ++i)
                {
                    if (types[i] == blockType)
                    {
                        ArrayUtility.Sub<Type>(ref types, i + 1);
                        break;
                    }
                }
            }
            return types;
        }

        public static T GetMemberValue<T>(string memberName, object target)
        {
            object value = GetMemberValue(memberName, target);
            if (value != null)
            {
                return (T)value;
            }
            return default;
        }

        public static object GetMemberValue(string memberName, object target)
        {
            if (string.IsNullOrEmpty(memberName) || target == null)
            {
                return null;
            }

            Type type = target.GetType();

            FieldInfo fieldInfo = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (fieldInfo != null)
            {
                if (fieldInfo.IsStatic)
                {
                    return fieldInfo.GetValue(null);
                }
                else
                {
                    return fieldInfo.GetValue(target);
                }
            }

            PropertyInfo propertyInfo = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (propertyInfo != null && propertyInfo.CanRead)
            {
                if (propertyInfo.GetGetMethod().IsStatic)
                {
                    propertyInfo.GetValue(null);
                }
                else
                {
                    return propertyInfo.GetValue(target);
                }
            }

            MethodInfo methodInfo = type.GetMethod(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (methodInfo != null)
            {
                if (methodInfo.IsStatic)
                {
                    methodInfo.Invoke(null, null);
                }
                else
                {
                    return methodInfo.Invoke(target, null);
                }
            }
            return null;
        }

        public static bool Compare(object value1, object value2, CompareSymbol symbol)
        {
            if (value1 == null && value2 == null)
            {
                if (symbol == CompareSymbol.Eq || symbol == CompareSymbol.Gte || symbol == CompareSymbol.Lte)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (value1 == null || value2 == null)
            {
                if (symbol == CompareSymbol.Neq)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if(value1.Equals(value2))
            {
                if (symbol == CompareSymbol.Eq || symbol == CompareSymbol.Gte || symbol == CompareSymbol.Lte)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if(value1 is IComparable && value2 is IComparable)
            {
                int compared = ((IComparable)value1).CompareTo((IComparable)value2);
                if (compared == 0)
                {
                    if (symbol == CompareSymbol.Eq || symbol == CompareSymbol.Gte || symbol == CompareSymbol.Lte)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (symbol == CompareSymbol.Neq)
                    {
                        return true;
                    }

                    if (compared > 0)
                    {
                        if (symbol == CompareSymbol.Gt || symbol == CompareSymbol.Gte)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (compared < 0)
                    {
                        if (symbol == CompareSymbol.Lt || symbol == CompareSymbol.Lte)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }else
            {
                if (symbol == CompareSymbol.Neq)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}