using System;

namespace DotEngine.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomDrawerEditorAttribute : Attribute
    {
        public bool IsShowScroll { get; set; } = true;
        public bool IsShowInherit { get; set; } = true;
        public int LabelWidth { get; set; } = 120;
    }
}
