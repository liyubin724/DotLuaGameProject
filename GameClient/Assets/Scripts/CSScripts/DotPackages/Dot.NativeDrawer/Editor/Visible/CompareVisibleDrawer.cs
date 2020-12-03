using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [Binder(typeof(CompareVisibleAttribute))]
    public class CompareVisibleDrawer : VisibleDrawer
    {
        public override bool IsVisible()
        {
            CompareVisibleAttribute attr = GetAttr<CompareVisibleAttribute>();

            object value = attr.Value;
            if (!string.IsNullOrEmpty(attr.MemberName))
            {
                value = DrawerUtility.GetMemberValue(attr.MemberName, Property.Target);
            }
            return DrawerUtility.Compare(Property.Value, value, attr.Symbol);
        }
    }
}
