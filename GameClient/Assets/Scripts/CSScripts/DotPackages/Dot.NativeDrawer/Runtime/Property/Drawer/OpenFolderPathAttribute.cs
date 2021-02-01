using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OpenFolderPathAttribute : PropertyContentAttribute
    {
        public bool IsAbsolute { get; set; } = false;

        public OpenFolderPathAttribute()
        {

        }
    }
}
