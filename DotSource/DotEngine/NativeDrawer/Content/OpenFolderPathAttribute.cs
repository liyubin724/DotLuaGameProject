using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OpenFolderPathAttribute : ContentAttribute
    {
        public bool IsAbsolute { get; set; } = false;

        public OpenFolderPathAttribute()
        {

        }
    }
}
