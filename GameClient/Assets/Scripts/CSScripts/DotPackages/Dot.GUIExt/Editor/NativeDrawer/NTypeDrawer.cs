using System;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class NTypeDrawer : NLayoutDrawer
    {
        internal string Label { get; set; }
        internal NFieldDrawer FieldDrawer { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomTypeDrawerAttribute : Attribute
    {
        public Type TargetType { get; private set; }

        public CustomTypeDrawerAttribute(Type type)
        {
            TargetType = type;
        }
    }
}