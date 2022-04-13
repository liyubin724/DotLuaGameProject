using DotEngine.GUIExt.NativeDrawer;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(CompareVisibleAttribute))]
    public class CompareVisibleAttrDrawer : VisibleAttrDrawer
    {
        public override bool IsVisible()
        {
            CompareVisibleAttribute attr = GetAttr<CompareVisibleAttribute>();
            if(string.IsNullOrEmpty(attr.MemberName))
            {
                return false;
            }else
            {
                SystemObject result = DrawerUtility.GetMemberValue(attr.MemberName, ItemDrawer.Target);
                return DrawerUtility.Compare(result, attr.Value, attr.Symbol);
            }
        }
    }
}
