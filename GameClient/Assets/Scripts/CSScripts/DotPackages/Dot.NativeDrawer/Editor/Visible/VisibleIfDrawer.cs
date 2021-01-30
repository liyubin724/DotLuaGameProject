using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [Binder(typeof(VisibleIfAttribute))]
    public class VisibleIfDrawer : VisibleDrawer
    {
        public override bool IsVisible()
        {
            VisibleIfAttribute attr = GetAttr<VisibleIfAttribute>();

            if(!string.IsNullOrEmpty(attr.MemberName))
            {
                return DrawerUtility.GetMemberValue<bool>(attr.MemberName, Property.Target);
            }
            return false;
        }
    }
}
