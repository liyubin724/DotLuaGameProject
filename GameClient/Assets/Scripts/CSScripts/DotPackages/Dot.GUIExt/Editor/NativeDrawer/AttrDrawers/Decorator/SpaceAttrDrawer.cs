using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class SpaceAttrDrawer : DecoratorAttrDrawer
    {
        public override void OnGUILayout()
        {
            SpaceAttribute attr = GetAttr<SpaceAttribute>();

            if (attr.Direction == SpaceDirection.Horizontal)
            {
                EGUILayout.DrawHorizontalSpace(attr.Size);
            }
            else
            {
                EGUILayout.DrawVerticalSpace(attr.Size);
            }
        }
    }
}

