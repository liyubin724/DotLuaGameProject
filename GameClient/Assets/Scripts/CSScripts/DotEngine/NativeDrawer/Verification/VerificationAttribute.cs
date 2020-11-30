using DotEngine.NativeDrawer.Condition;

namespace DotEngine.NativeDrawer.Verification
{
    public abstract class VerificationCompareAttribute : CompareAttribute
    {
        public string ValidMsg { get; private set; }
        public string UnvalidMsg { get; private set; }

        protected VerificationCompareAttribute(string memberName, object value, CompareSymbol symbol,string validMsg,string unvalidMsg) : base(memberName, value, symbol)
        {
            ValidMsg = validMsg;
            UnvalidMsg = unvalidMsg;
        }
    }
}
