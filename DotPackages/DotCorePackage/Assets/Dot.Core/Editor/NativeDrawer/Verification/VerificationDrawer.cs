using DotEngine.NativeDrawer.Verification;

namespace DotEditor.NativeDrawer.Verification
{
    public abstract class VerificationDrawer : CompareAttrNativeDrawer
    {
        protected VerificationDrawer(object target, VerificationCompareAttribute attr) : base(target, attr)
        {
        }

        public abstract void OnGUILayout();
    }
}
