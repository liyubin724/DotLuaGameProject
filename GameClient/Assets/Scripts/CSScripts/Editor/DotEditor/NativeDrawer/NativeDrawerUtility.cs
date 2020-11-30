using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Layout;
using DotEngine.NativeDrawer.Listener;
using DotEngine.NativeDrawer.Property;
using DotEngine.NativeDrawer.Verification;
using DotEngine.NativeDrawer.Visible;
using DotEngine.Utilities;
using DotEditor.NativeDrawer.Decorator;
using DotEditor.NativeDrawer.Layout;
using DotEditor.NativeDrawer.Listener;
using DotEditor.NativeDrawer.Property;
using DotEditor.NativeDrawer.Verification;
using DotEditor.NativeDrawer.Visible;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    public static class NativeDrawerUtility
    {
        private static Dictionary<Type, Type> attrDrawerDic = new Dictionary<Type, Type>();

        private static Dictionary<Type, Type> defaultTypeDrawerDic = new Dictionary<Type, Type>();

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
                                where !type.IsAbstract && !type.IsInterface && typeof(AttrNativeDrawer).IsAssignableFrom(type) 
                                select type
                                ).ToArray();
                foreach(var type in types)
                {
                    AttrDrawBinderAttribute attr = type.GetCustomAttribute<AttrDrawBinderAttribute>();
                    if(attr!=null)
                    {
                        attrDrawerDic.Add(attr.AttrType, type);
                    }
                }

                types = (
                        from type in assembly.GetTypes()
                        where !type.IsAbstract && !type.IsInterface && typeof(NativeTypeDrawer).IsAssignableFrom(type)
                        select type
                        ).ToArray();
                foreach(var type in types)
                {
                    CustomTypeDrawerAttribute attr = type.GetCustomAttribute<CustomTypeDrawerAttribute>();
                    if(attr!=null)
                    {
                        defaultTypeDrawerDic.Add(attr.Target, type);
                    }
                }
            }
        }

        public static object CreateDefaultInstance(Type type)
        {
            if(type.IsArray)
            {
                return Array.CreateInstance(TypeUtility.GetArrayOrListElementType(type), 0);
            }
            if(type == typeof(string))
            {
                return string.Empty;
            }
            if(typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                if(typeof(ScriptableObject).IsAssignableFrom(type))
                {
                    return ScriptableObject.CreateInstance(type);
                }
                return null;
            }

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

        public static bool IsTypeSupported(Type type)
        {
            if(type.IsInterface)
            {
                return false;
            }

            if(type.IsDelegate())
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

        public static Type GetDefaultType(Type type)
        {
            if(type.IsEnum)
            {
                return typeof(Enum);
            }
            if(TypeUtility.IsArrayOrList(type))
            {
                return typeof(IList);
            }
            
            if(typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return typeof(UnityEngine.Object);
            }
            return type;
        }

        public static NativeTypeDrawer CreateDefaultTypeDrawer(NativeDrawerProperty property)
        {
            Type type = GetDefaultType(property.ValueType);
            if (defaultTypeDrawerDic.TryGetValue(type, out Type drawerType))
            {
                return (NativeTypeDrawer)Activator.CreateInstance(drawerType, property);
            }
            return null;
        }

        public static DecoratorDrawer CreateDecoratorDrawer(NativeDrawerProperty drawerProperty, DecoratorAttribute attr)
        {
            if(attrDrawerDic.TryGetValue(attr.GetType(),out Type drawerType))
            {
                return (DecoratorDrawer)Activator.CreateInstance(drawerType, drawerProperty, attr);
            }
            return null;
        }

        public static LayoutDrawer CreateLayoutDrawer(LayoutAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (LayoutDrawer)Activator.CreateInstance(drawerType, attr);
            }
            return null;
        }

        public static VerificationDrawer CreateVerificationDrawer(object target, VerificationCompareAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (VerificationDrawer)Activator.CreateInstance(drawerType, target,attr);
            }
            return null;
        }

        public static VisibleDrawer CreateVisibleDrawer(VisibleAtrribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (VisibleDrawer)Activator.CreateInstance(drawerType, attr);
            }
            return null;
        }

        public static VisibleCompareDrawer CreateVisibleCompareDrawer(object target, VisibleCompareAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (VisibleCompareDrawer)Activator.CreateInstance(drawerType,target,attr);
            }
            return null;
        }

        public static PropertyDrawer CreatePropertyDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (PropertyDrawer)Activator.CreateInstance(drawerType, drawerProperty, attr );
            }
            return null;
        }

        public static PropertyLabelDrawer CreatePropertyLabelDrawer(PropertyLabelAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (PropertyLabelDrawer)Activator.CreateInstance(drawerType, attr);
            }
            return null;
        }

        public static PropertyControlDrawer CreatePropertyControlDrawer(PropertyControlAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (PropertyControlDrawer)Activator.CreateInstance(drawerType, attr);
            }
            return null;
        }

        public static ListenerDrawer CreateListenerDrawer(object target,ListenerAttribute attr)
        {
            if (attrDrawerDic.TryGetValue(attr.GetType(), out Type drawerType))
            {
                return (ListenerDrawer)Activator.CreateInstance(drawerType, target,attr);
            }
            return null;
        }

        public static T GetMemberValue<T>(string memberName, object target)
        {
            return (T)GetMemberValue(memberName, target);
        }

        public static object GetMemberValue(string memberName,object target)
        {
            if (string.IsNullOrEmpty(memberName) || target == null)
            {
                return null;
            }

            Type type = target.GetType();

            FieldInfo fieldInfo = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(target);
            }

            PropertyInfo propertyInfo = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(target);
            }

            MethodInfo methodInfo = type.GetMethod(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo != null)
            {
                return methodInfo.Invoke(target, null);
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

            Type[] types = type.GetAllBasedTypes();
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
    }
}
