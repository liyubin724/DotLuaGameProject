using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndIndentAttribute : LayoutAttribute
    {
        public EndIndentAttribute(LayoutOccasion occasion = LayoutOccasion.After) : base(occasion)
        {
        }
    }
}
