using DotEngine.NativeDrawer.Verification;

namespace DotEditor.NativeDrawer.Verification
{
    [Binder(typeof(VerificationIfAttribute))]
    public class VerificationIfDrawer : VerificationDrawer
    {
        public override bool IsValid()
        {
            VerificationIfAttribute attr = GetAttr<VerificationIfAttribute>();
            if(string.IsNullOrEmpty(attr.MemberName))
            {
                return false;
            }

            return DrawerUtility.GetMemberValue<bool>(attr.MemberName, Property.Target);
        }
    }
}
