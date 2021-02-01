using System;

namespace DotEngine.NativeDrawer.Layout
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginGroupAttribute : LayoutAttribute
    {
        public string Label { get; private set; }
        public BeginGroupAttribute(string label = null)
        {
            Label = label;
        }
    }
}
