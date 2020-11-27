namespace DotEngine.NativeDrawer.Verification
{
    //public abstract class VerificationAttribute : NativeDrawerAttribute
    //{
    //}

    public abstract class VerificationCompareAttribute : CompareDrawerAttribute
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
