using System;

namespace DotEngine.NativeDrawer.Property
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FloatSliderAttribute : PropertyContentAttribute
    {
        public float LeftValue { get; private set; }
        public float RightValue { get; private set; }

        public string LeftValueMemberName { get; private set; }
        public string RightValueMemberName { get; private set; }

        public FloatSliderAttribute(float left,float right) 
        {
            LeftValue = left;
            RightValue = right;
        }

        public FloatSliderAttribute(string leftMemberName,string rightMemberName)
        {
            LeftValueMemberName = leftMemberName;
            RightValueMemberName = rightMemberName;
        }
    }
}
