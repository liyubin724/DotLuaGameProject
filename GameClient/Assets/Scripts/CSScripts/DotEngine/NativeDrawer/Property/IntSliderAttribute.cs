using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IntSliderAttribute : PropertyDrawerAttribute
    {
        public int LeftValue { get; set; } = int.MinValue;
        public int RightValue { get; set; } = int.MaxValue;

        public string LeftValueMemberName { get; set; } = null;
        public string RightValueMemberName { get; set; } = null;

        public IntSliderAttribute()
        {

        }
    }
}
