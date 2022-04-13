using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class IntSliderAttribute : ContentAttribute
    {
        public int LeftValue { get; private set; }
        public int RightValue { get; private set; }

        public string LeftValueMemberName { get; private set; }
        public string RightValueMemberName { get; private set; }

        public IntSliderAttribute(int left, int right)
        {
            LeftValue = left;
            RightValue = right;
        }

        public IntSliderAttribute(string leftMemberName, string rightMemberName)
        {
            LeftValueMemberName = leftMemberName;
            RightValueMemberName = rightMemberName;
        }
    }
}
