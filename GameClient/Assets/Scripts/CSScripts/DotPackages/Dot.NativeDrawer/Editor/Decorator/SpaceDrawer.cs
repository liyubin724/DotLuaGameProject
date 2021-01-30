using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Decorator;

namespace DotEditor.NativeDrawer.Decorator
{
    [Binder(typeof(SpaceAttribute))]
    public class SpaceDrawer : DecoratorDrawer
    {
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
