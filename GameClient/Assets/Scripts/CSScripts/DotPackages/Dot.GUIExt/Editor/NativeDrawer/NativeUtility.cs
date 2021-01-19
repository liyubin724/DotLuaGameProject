using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public static class NativeUtility
    {
        private static Dictionary<Type, Type> defaultTypeDrawerDic = null;

        private static void LoadTypeDrawers()
        {
            defaultTypeDrawerDic = new Dictionary<Type, Type>();
            Type[] types = AssemblyUtility.GetDerivedTypes(typeof(NTypeDrawer));
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

        public static NLayoutDrawer GetLayoutDrawer(SystemObject value)
        {
            Type type = value.GetType();

            NLayoutDrawer drawer = CreateTypeDrawer(type);
            if(drawer == null)
            {
                if (TypeUtility.IsArrayOrListType(type))
                {
                    drawer = new NArrayDrawer(value);
                }
                else if (TypeUtility.IsStructOrClassType(type))
                {
                    drawer = new NObjectDrawer(value);
                }
            }
            return drawer;
        }

        public static NTypeDrawer CreateTypeDrawer(Type type)
        {
            if (defaultTypeDrawerDic == null)
            {
                LoadTypeDrawers();
            }

            Type targetType = type.IsEnum?typeof(Enum):type;
            if (defaultTypeDrawerDic.TryGetValue(targetType, out var drawerType) && drawerType != null)
            {
                return (NTypeDrawer)Activator.CreateInstance(drawerType);
            }
            return null;
        }

        public static SystemObject CreateTypeInstance(Type type)
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
    }
}