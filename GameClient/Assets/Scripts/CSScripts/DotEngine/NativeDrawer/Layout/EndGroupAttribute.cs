using System;

namespace DotEngine.NativeDrawer.Layout
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndGroupAttribute : LayoutAttribute
    {
        public EndGroupAttribute(LayoutOccasion occasion = LayoutOccasion.Before) : base(occasion)
        {

        }
    }
}
