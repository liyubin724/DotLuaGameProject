using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumButtonAttribute : PropertyContentAttribute
    {
        public float MinWidth { get; set; } = -1;
        public float MaxWidth { get; set; } = -1;

        public EnumButtonAttribute()
        {
        }
    }
}
