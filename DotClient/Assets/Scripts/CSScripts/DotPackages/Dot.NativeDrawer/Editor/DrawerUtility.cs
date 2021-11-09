using DotEditor.NativeDrawer.Property;
using DotEngine.Core.Extensions;
using DotEngine.Core.Utilities;
using DotEngine.NativeDrawer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace DotEditor.NativeDrawer
{
    public static class DrawerUtility
    {
        private static Dictionary<Type, Type> attrDrawerDic = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> customTypeDrawerDic = new Dictionary<Type, Type>();

        [UnityEditor.InitializeOnLoadMethod]
        public static void OnDrawerInited()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                string assemblyName = assembly.GetName().Name;
                if(assemblyName.StartsWith("Unity") || assemblyName.StartsWith("System") || assemblyName.StartsWith("Mono") )
                {
                    continue;
                }
                Type[] types = (
                                from type in assembly.GetTypes() 
                                where !type.IsAbstract && !type.IsInterface && typeof(Drawer).IsAssignableFrom(type)
                                select type
                                ).ToArray();
                foreach(var type in types)
                {
                    BinderAttribute binderAttr = type.GetCustomAttribute<BinderAttribute>();
                    if(binderAttr != null)
                    {
                        attrDrawerDic.Add(binderAttr.AttrType, type);
                    }

                    CustomTypeDrawerAttribute drawerAttr = type.GetCustomAttribute<CustomTypeDrawerAttribute>();
                    if (drawerAttr != null)
                    {
                        customTypeDrawerDic.Add(drawerAttr.Target, type);
                    }
                }
            }
        }

        public static object CreateInstance(Type type)
        {
            if(type.IsArray)
            {
                return Array.CreateInstance(type.GetElementTypeInArrayOrList(), 0);
            }else if(type == typeof(string))
            {
                return string.Empty;
            }else if(typeof(UnityObject).IsAssignableFrom(type))
            {
                if(typeof(ScriptableObject).IsAssignableFrom(type))
                {
                    return ScriptableObject.CreateInstance(type);
                }
                return null;
            }else
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

        public static bool IsTypeSupported(Type type)
        {
            if(type.IsInterface)
            {
                return false;
            }

            if(type.IsDelegateType())
            {
                return false;
            }

            if(type.IsClass && type.IsAbstract)
            {
                return false;
            }

            if(typeof(IDictionary).IsAssignableFrom(type))
            {
                return false;
            }
            return true;
        }

        public static Type GetReallyDrawerType(Type type)
        {
            if(type.IsEnum)
            {
                return typeof(Enum);
            }
            if(type.IsArrayOrListType())
            {
                return typeof(IList);
            }
            
            if(typeof(UnityObject).IsAssignableFrom(type))
            {
                return typeof(UnityObject);
            }

            return type;
        }

        public static PropertyContentDrawer CreateCustomTypeDrawer(DrawerProperty property)
        {
            Type type = GetReallyDrawerType(property.ValueType);
            if (customTypeDrawerDic.TryGetValue(type, out Type drawerType))
            {
                PropertyContentDrawer drawer = (PropertyContentDrawer)Activator.CreateInstance(drawerType);
                drawer.Property = property;

                return drawer;
            }

            return null;
        }

        public static Drawer CreateAttrDrawer(DrawerProperty property, DrawerAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                Drawer drawer = (Drawer)Activator.CreateInstance(drawerType);
                drawer.Attr = attr;
                drawer.Property = property;

                return drawer;
            }

            return null;
        }

        public static T GetMemberValue<T>(string memberName, object target)
        {
            object value = GetMemberValue(memberName, target);
            if(value!=null)
            {
                return (T)value;
            }
            return default;
        }

        public static object GetMemberValue(string memberName,object target)
        {
            if (string.IsNullOrEmpty(memberName) || target == null)
            {
                return null;
            }

            Type type = target.GetType();

            FieldInfo fieldInfo = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (fieldInfo != null)
            {
                if(fieldInfo.IsStatic)
                {
                    return fieldInfo.GetValue(null);
                }else
                {
                    return fieldInfo.GetValue(target);
                }
            }

            PropertyInfo propertyInfo = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (propertyInfo != null && propertyInfo.CanRead)
            {
                if(propertyInfo.GetGetMethod().IsStatic)
                {
                    propertyInfo.GetValue(null);
                }else
                {
                    return propertyInfo.GetValue(target);
                }
            }

            MethodInfo methodInfo = type.GetMethod(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (methodInfo != null)
            {
                if(methodInfo.IsStatic)
                {
                    methodInfo.Invoke(null, null);
                }else
                {
                    return methodInfo.Invoke(target, null);
                }
            }
            return null;
        }

        public static Type[] GetAllBaseTypes(Type type)
        {
            if(type.IsValueType)
            {
                return new Type[] { type };
            }
            if(type.IsArray)
            {
                return new Type[] { type };
            }
            if(type.IsEnum)
            {
                return new Type[] { type};
            }
            if (typeof(List<>).IsAssignableFrom(type))
            {
                return new Type[] { type };
            }

            Type[] types = type.GetBaseTypes();
            if(types!=null && types.Length>0)
            {
                Type blockType;
                if(type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    blockType = typeof(MonoBehaviour);
                }else if(type.IsSubclassOf(typeof(ScriptableObject)))
                {
                    blockType = typeof(ScriptableObject);
                }else if(type.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    blockType = typeof(UnityEngine.Object);
                }else
                {
                    blockType = typeof(System.Object);
                }

                for(int i =0;i<types.Length;++i)
                {
                    if(types[i] == blockType)
                    {
                        ArrayUtility.Sub<Type>(ref types, i+1);
                        break;
                    }
                }
            }
            return types;
        }

        public static bool Compare(object value1,object value2,CompareSymbol symbol)
        {
            if(value1 == null && value2 == null)
            {
                if (symbol == CompareSymbol.Eq || symbol == CompareSymbol.Gte || symbol == CompareSymbol.Lte)
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }else if(value1 == null || value2 ==null)
            {
                if(symbol == CompareSymbol.Neq)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }else
            {
                if (value1.GetType() != value2.GetType())
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
                else
                {
                    if (value1.GetType().CanCastableTo(typeof(IComparable)))
                    {
                        int compared = ((IComparable)value1).CompareTo((IComparable)value2);
                        if(compared == 0)
                        {
                            if (symbol == CompareSymbol.Eq || symbol == CompareSymbol.Gte || symbol == CompareSymbol.Lte)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }else
                        {
                            if(symbol == CompareSymbol.Neq)
                            {
                                return true;
                            }

                            if(compared>0)
                            {
                                if(symbol == CompareSymbol.Gt || symbol == CompareSymbol.Gte)
                                {
                                    return true;
                                }else
                                {
                                    return false;
                                }
                            }else if(compared<0)
                            {
                                if (symbol == CompareSymbol.Lt || symbol == CompareSymbol.Lte)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }else
                            {
                                return false;
                            }
                        }
                    }else
                    {
                        if(value1 == value2)
                        {
                            if (symbol == CompareSymbol.Eq || symbol == CompareSymbol.Gte || symbol == CompareSymbol.Lte)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }else
                        {
                            if(symbol == CompareSymbol.Neq)
                            {
                                return true;
                            }else
                            {
                                return false;
                            }
                        }
                    }//endof TypeUtility.IsCastableTo
                }
            }

        }
    }
}
