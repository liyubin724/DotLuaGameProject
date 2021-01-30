using DotEngine.NativeDrawer.Decorator;
using DotEditor.GUIExtension;

namespace DotEditor.NativeDrawer.Decorator
{
    [CustomAttributeDrawer(typeof(SeparatorLineAttribute))]
    public class SeparatorLineDrawer : DecoratorDrawer
    {
        public SeparatorLineDrawer(NativeDrawerProperty property, DecoratorAttribute attr) : base(property, attr)
        {
        }

        public override void OnGUILayout()
        {
            EGUILayout.DrawHorizontalLine();
        }
    }
}
