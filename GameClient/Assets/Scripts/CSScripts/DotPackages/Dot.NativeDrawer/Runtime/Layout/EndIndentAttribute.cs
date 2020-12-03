using System;

namespace DotEngine.NativeDrawer.Layout
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndIndentAttribute : LayoutAttribute
    {
        public EndIndentAttribute(LayoutOccasion occasion = LayoutOccasion.After) : base(occasion)
        {

        }
    }
}
