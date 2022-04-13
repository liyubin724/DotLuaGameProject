using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EndHorizontalAttribute : LayoutAttribute
    {
        public EndHorizontalAttribute(LayoutOccasion occasion = LayoutOccasion.After) : base(occasion)
        {

        }
    }
}