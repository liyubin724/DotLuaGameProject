using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [Binder(typeof(VisibleIfAttribute))]
    public class VisibleIfDrawer : VisibleDrawer
    {
        public override bool IsVisible()
        {
            VisibleIfAttribute attr = GetAttr<VisibleIfAttribute>();

            if(!string.IsNullOrEmpty(attr.VisibleMemberName))
            {
                return DrawerUtility.GetMemberValue<bool>(attr.VisibleMemberName, Property.Target);
            }
            return false;
        }
    }
}
