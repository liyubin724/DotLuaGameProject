namespace DotEngine.GUIExt.NativeDrawer
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
