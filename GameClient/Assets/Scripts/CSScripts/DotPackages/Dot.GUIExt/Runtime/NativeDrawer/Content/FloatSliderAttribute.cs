using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FloatSliderAttribute : ContentAttribute
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public string MinValueMemberName { get; private set; }
        public string MaxValueMemberName { get; private set; }
        public FloatSliderAttribute(float min, float max)
        {
            MinValue = min;
            MaxValue = max;
        }

        public FloatSliderAttribute(string minMemberName, string maxMemberName)
        {
            MinValueMemberName = minMemberName;
            MaxValueMemberName = maxMemberName;
        }
    }
}

