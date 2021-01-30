using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FloatSliderAttribute : PropertyDrawerAttribute
    {
        public float LeftValue { get; set; } = float.MinValue;
        public float RightValue { get; set; } = float.MaxValue;

        public string LeftValueMemberName { get; set; } = null;
        public string RightValueMemberName { get; set; } = null;

        public FloatSliderAttribute() { }
    }
}
