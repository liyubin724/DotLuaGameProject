using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(VisibleIfAttribute))]
    public class VisibleIfAttrDrawer : VisibleAttrDrawer
    {
        public override bool IsVisible()
        {
            VisibleIfAttribute attr = GetAttr<VisibleIfAttribute>();

            if (!string.IsNullOrEmpty(attr.MemberName))
            {
                return DrawerUtility.GetMemberValue<bool>(attr.MemberName, ItemDrawer.Target);
            }
            return false;
        }
    }
}
