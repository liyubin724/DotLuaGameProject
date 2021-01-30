using System;

namespace DotEngine.NativeDrawer.Decorator
{
    public enum ButtonSize
    {
        Big,
        Normal,
        Small,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ButtonAttribute : DecoratorAttribute
    {
        public ButtonSize Size { get; set; } = ButtonSize.Normal;
        public string Label { get; set; }
        public string MethodName { get; set; }

        public ButtonAttribute(string methodName) 
        {
            MethodName = methodName;
        }
    }
}
