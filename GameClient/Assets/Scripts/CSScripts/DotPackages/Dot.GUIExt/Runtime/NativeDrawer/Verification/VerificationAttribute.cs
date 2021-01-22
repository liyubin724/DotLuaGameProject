namespace DotEngine.GUIExt.NativeDrawer
{
    public abstract class VerificationAttribute : NDrawerAttribute
    {
        public string InvalidMsg { get; private set; }

        public VerificationAttribute(string invalidMsg)
        {
            InvalidMsg = invalidMsg;
        }
    }
}
