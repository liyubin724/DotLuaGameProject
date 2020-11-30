using System;

namespace DotEngine.NativeDrawer.Layout
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndHorizontalAttribute : LayoutAttribute
    {
        public EndHorizontalAttribute(LayoutOccasion occasion = LayoutOccasion.Before) : base(occasion)
        {

        }
    }
}
