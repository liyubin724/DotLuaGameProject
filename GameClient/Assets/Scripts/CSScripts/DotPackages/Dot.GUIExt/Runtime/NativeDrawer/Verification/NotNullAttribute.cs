using System;

namespace DotEngine.GUIExt.NativeDrawer
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NotNullAttribute : VerificationAttribute
    {
        public NotNullAttribute(string invalidMsg) : base(invalidMsg)
        {
        }
    }
}

