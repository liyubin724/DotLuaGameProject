namespace DotEngine.NativeDrawer.Verification
{
    public class VerificationIfAttribute : VerificationAttribute
    {
        public string MemberName { get; private set; }

        public VerificationIfAttribute(string memberName, string invalidMsg) : base(invalidMsg)
        {
            MemberName = memberName;
        }
    }
}
