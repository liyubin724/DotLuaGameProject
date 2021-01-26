using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumButtonAttribute : ContentAttribute
    {
        public float MinWidth { get; set; } = -1;
        public float MaxWidth { get; set; } = -1;

        public bool IsExpandWidth { get; set; } = true;

        public EnumButtonAttribute()
        {
        }
    }
}
