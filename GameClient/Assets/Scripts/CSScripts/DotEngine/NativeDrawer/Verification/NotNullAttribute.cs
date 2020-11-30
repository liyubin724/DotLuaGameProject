using DotEngine.NativeDrawer.Condition;
using System;

namespace DotEngine.NativeDrawer.Verification
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NotNullAttribute : VerificationCompareAttribute
    {
        public NotNullAttribute(string memberName,string unvalidMsg) : base(memberName, null, CompareSymbol.Eq,null,unvalidMsg)
        {
        }
    }
}
