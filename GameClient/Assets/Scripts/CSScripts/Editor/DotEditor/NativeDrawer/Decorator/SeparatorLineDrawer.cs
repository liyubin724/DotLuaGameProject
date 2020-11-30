using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Decorator;

namespace DotEditor.NativeDrawer.Decorator
{
    [AttrBinder(typeof(SeparatorLineAttribute))]
    public class SeparatorLineDrawer : DecoratorDrawer
    {
        public SeparatorLineDrawer(NativeDrawerProperty property, DecoratorAttribute attr) : base(property, attr)
        {
        }

        public override void OnGUILayout()
        {
            SeparatorLineAttribute attr = GetAttr<SeparatorLineAttribute>();
            EGUILayout.DrawHorizontalLine(attr.Color,attr.Thickness,attr.Padding);
        }
    }
}
