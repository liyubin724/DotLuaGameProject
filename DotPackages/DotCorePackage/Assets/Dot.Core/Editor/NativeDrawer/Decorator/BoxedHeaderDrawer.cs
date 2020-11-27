using DotEngine.NativeDrawer.Decorator;
using DotEditor.GUIExtension;
using UnityEngine;

namespace DotEditor.NativeDrawer.Decorator
{
    [CustomAttributeDrawer(typeof(BoxedHeaderAttribute))]
    public class BoxedHeaderDrawer : DecoratorDrawer
    {
        public BoxedHeaderDrawer(NativeDrawerProperty property, DecoratorAttribute attr) : base(property, attr)
        {
        }

        public override void OnGUILayout()
        {
            BoxedHeaderAttribute attr = GetAttr<BoxedHeaderAttribute>();
            EGUILayout.DrawBoxHeader(attr.Header, GUILayout.ExpandWidth(true));
        }
    }
}
