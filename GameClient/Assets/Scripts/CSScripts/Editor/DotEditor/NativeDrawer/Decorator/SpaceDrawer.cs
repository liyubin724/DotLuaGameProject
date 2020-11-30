using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Decorator;

namespace DotEditor.NativeDrawer.Decorator
{
    [AttrDrawBinder(typeof(SpaceAttribute))]
    public class SpaceDrawer : DecoratorDrawer
    {
        public SpaceDrawer(NativeDrawerProperty property, DecoratorAttribute attr) : base(property, attr)
        {
        }

        public override void OnGUILayout()
        {
            SpaceAttribute attr = GetAttr<SpaceAttribute>();

            if(attr.Direction == SpaceDirection.Horizontal)
            {
                EGUILayout.DrawHorizontalSpace(attr.Size);
            }else
            {
                EGUILayout.DrawVerticalSpace(attr.Size);
            }
        }
    }
}
