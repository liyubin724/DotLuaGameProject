namespace DotEngine.NativeDrawer.Verification
{
    public abstract class VerificationAttribute : DrawerAttribute
    {
        public string InvalidMsg { get; private set; }

        public VerificationAttribute(string invalidMsg)
        {
            InvalidMsg = invalidMsg;
        }
    }
}
