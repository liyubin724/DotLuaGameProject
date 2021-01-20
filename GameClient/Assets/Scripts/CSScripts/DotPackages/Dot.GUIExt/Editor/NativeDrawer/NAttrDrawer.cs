using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class NAttrDrawer : NLayoutDrawer
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomAttrDrawerAttribute : Attribute
    {
    }
}