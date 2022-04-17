using System;

namespace DotEngine.NativeDrawer.Layout
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginIndentAttribute : LayoutAttribute
    {
        public BeginIndentAttribute(LayoutOccasion occasion = LayoutOccasion.Before) : base(occasion)
        {
        }
    }
}
