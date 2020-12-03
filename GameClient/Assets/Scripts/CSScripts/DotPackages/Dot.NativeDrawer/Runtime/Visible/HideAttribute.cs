using System;

namespace DotEngine.NativeDrawer.Visible
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =true)]
    public class HideAttribute : VisibleAtrribute
    {
    }
}
