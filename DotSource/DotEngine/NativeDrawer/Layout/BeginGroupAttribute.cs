using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginGroupAttribute : LayoutAttribute
    {
        public string Label { get; private set; }
        public BeginGroupAttribute(string label = null, LayoutOccasion occasion = LayoutOccasion.Before) : base(occasion)
        {
            Label = label;
        }
    }
}
