using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndGroupAttribute : LayoutAttribute
    {
        public EndGroupAttribute(LayoutOccasion occasion = LayoutOccasion.After) : base(occasion)
        {
        }
    }
}
