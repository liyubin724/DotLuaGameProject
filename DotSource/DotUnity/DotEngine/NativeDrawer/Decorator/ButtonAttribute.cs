using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    public enum ButtonSize
    {
        Big = 0,
        Normal,
        Small,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ButtonAttribute : DecoratorAttribute
    {
        public ButtonSize Size { get; set; } = ButtonSize.Normal;
        public string Label { get; set; }
        public string MethodName { get; private set; }

        public ButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}
