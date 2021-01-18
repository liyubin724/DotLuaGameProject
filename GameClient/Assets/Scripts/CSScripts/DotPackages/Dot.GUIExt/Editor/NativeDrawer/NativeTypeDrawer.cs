using System;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class NativeTypeDrawer
    {
        public abstract float GetHeight();
        public abstract void OnGUI(Rect rect, string label, NativeField field);
    }

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class NativeTypeDrawerAttribute : Attribute
    {
        public Type TargetType { get; private set; }

        public NativeTypeDrawerAttribute(Type type)
        {
            TargetType = type;
        }
    }
}
