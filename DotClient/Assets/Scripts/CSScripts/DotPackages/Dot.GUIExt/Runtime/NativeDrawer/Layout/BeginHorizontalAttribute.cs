using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BeginHorizontalAttribute : LayoutAttribute
    {
        public BeginHorizontalAttribute(LayoutOccasion occasion = LayoutOccasion.Before) : base(occasion)
        {
        }
    }
}
