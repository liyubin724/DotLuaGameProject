using DotEngine.NativeDrawer.Verification;

namespace DotEditor.NativeDrawer.Verification
{
    [Binder(typeof(NotNullAttribute))]
    public class NotNullDrawer : VerificationDrawer
    {
        public override bool IsValid()
        {
            return Property.Value != null;
        }
    }
}
