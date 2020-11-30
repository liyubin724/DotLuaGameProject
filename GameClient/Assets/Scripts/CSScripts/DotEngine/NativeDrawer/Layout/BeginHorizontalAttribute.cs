using System;

namespace DotEngine.NativeDrawer.Layout
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginHorizontalAttribute : LayoutAttribute
    {
        public BeginHorizontalAttribute(LayoutOccasion occasion = LayoutOccasion.Before) : base(occasion)
        { 
        }
    }
}
