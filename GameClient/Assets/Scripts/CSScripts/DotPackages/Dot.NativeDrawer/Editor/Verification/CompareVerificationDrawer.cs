using DotEngine.NativeDrawer.Verification;

namespace DotEditor.NativeDrawer.Verification
{
    [Binder(typeof(CompareVerificationAttribute))]
    public class CompareVerificationDrawer : VerificationDrawer
    {
        public override bool IsValid()
        {
            CompareVerificationAttribute attr = GetAttr<CompareVerificationAttribute>();

            object value = attr.Value;
            if(!string.IsNullOrEmpty(attr.MemberName))
            {
                value = DrawerUtility.GetMemberValue(attr.MemberName, Property.Target);
            }
            return DrawerUtility.Compare(Property.Value, value, attr.Symbol);
        }
    }
}
