using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OpenFilePathAttribute : PropertyContentAttribute
    {
        public bool IsAbsolute { get; set; } = false;

        public string Extension { get; set; } = null;

        //example:[ "Image files", "png,jpg,jpeg", "All files", "*" ]
        public string[] Filters { get; set; } = null;

        public OpenFilePathAttribute()
        {

        }
    }
}
