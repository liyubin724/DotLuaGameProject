using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginIndentAttribute : LayoutAttribute
    {
        public BeginIndentAttribute(LayoutOccasion occasion = LayoutOccasion.Before) : base(occasion)
        {
        }
    }
}