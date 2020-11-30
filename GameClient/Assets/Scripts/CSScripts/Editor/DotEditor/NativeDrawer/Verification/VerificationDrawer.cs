using DotEngine.NativeDrawer.Verification;

namespace DotEditor.NativeDrawer.Verification
{
    public abstract class VerificationDrawer : CompareAttrDrawer
    {
        protected VerificationDrawer(object target, VerificationCompareAttribute attr) : base(target, attr)
        {
        }

        public abstract void OnGUILayout();
    }
}
