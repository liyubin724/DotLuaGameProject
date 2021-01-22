using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class SeparatorLineAttrDrawer : DecoratorAttrDrawer
    {
        public override void OnGUILayout()
        {
            SeparatorLineAttribute attr = GetAttr<SeparatorLineAttribute>();
            EGUILayout.DrawHorizontalLine(attr.Color, attr.Thickness, attr.Padding);
        }
    }
}
