using DotEngine.GUIExt.NativeDrawer;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class BoxedHeaderDrawer : DecoratorAttrDrawer
    {
        public override void OnGUILayout()
        {
            BoxedHeaderAttribute attr = GetAttr<BoxedHeaderAttribute>();
            EGUILayout.DrawBoxHeader(attr.Header, GUILayout.ExpandWidth(true));
        }
    }
}
