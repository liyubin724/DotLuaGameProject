using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;
using DotEngine.Utilities;

namespace DotEditor.GUIExt.Field
{
    public static class FieldValueUtility
    {
        private static Dictionary<Type, Type> typeForDrawerDic = null;

        static void Load()
        {
            typeForDrawerDic = new Dictionary<Type, Type>();

            Type[] types = AssemblyUtility.GetDerivedTypes(typeof(FieldValueDrawer));
            foreach(var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(TypeDrawableAttribute), false);
                if(attrs!=null && attrs.Length>0)
                {
                    TypeDrawableAttribute attr = attrs[0] as TypeDrawableAttribute;
                    if(attr!=null)
                    {
                        typeForDrawerDic.Add(attr.ValueType, type);
                    }
                }
            }
        }

        public static FieldValueDrawer CreateDrawer(Type valueType)
        {
            if(typeForDrawerDic.TryGetValue(valueType,out var type) && type != null)
            {
                return (FieldValueDrawer)Activator.CreateInstance(type);
            }
            return null;
        }

        public static SystemObject CreateInstance(Type type)
        {
            if(type == typeof(string))
            {
                return string.Empty;
            }
            return Activator.CreateInstance(type);
        }
    }
}
