using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class CustomDrawerEditorAttribute : Attribute
    {
        public bool Enable { get; set; } = true;
        public bool IsShowScroll { get; set; } = true;
        public bool IsShowInherit { get; set; } = false;
        public bool IsShowBox { get; set; } = true;
        public string Header { get; set; } = string.Empty;
        public int LabelWidth { get; set; } = 0;

        public CustomDrawerEditorAttribute()
        {

        }
    }
}
