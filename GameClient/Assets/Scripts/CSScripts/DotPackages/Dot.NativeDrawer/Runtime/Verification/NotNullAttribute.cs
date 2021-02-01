using System;

namespace DotEngine.NativeDrawer.Verification
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NotNullAttribute : VerificationAttribute
    {
        public NotNullAttribute(string invalidMsg):base(invalidMsg)
        {
        }
    }
}
