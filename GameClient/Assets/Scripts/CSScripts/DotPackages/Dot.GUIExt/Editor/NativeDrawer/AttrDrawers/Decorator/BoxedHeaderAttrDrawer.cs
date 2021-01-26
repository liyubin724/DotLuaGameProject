using DotEditor.GUIExt.IMGUI;
using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(BoxedHeaderAttribute))]
    public class BoxedHeaderDrawer : DecoratorAttrDrawer
    {
        private HeaderDrawer drawer = null;
        public override void OnGUILayout()
        {
            if(drawer == null)
            {
                BoxedHeaderAttribute attr = GetAttr<BoxedHeaderAttribute>();
                drawer = new HeaderDrawer()
                {
                    Header = attr.Header,
                };
            }
            drawer.OnGUILayout();
        }
    }
}
