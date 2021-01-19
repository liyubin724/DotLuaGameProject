using System;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class NativeTypeDrawer
    {
        public abstract float GetHeight();
        public abstract void OnGUI(Rect rect, string label, NativeFieldDrawer field);
    }

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomTypeDrawerAttribute : Attribute
    {
        public Type TargetType { get; private set; }

        public CustomTypeDrawerAttribute(Type type)
        {
            TargetType = type;
        }
    }
}
