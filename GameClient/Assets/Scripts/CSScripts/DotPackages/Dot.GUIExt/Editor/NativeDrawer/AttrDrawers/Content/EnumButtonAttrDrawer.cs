using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class EnumButtonAttrDrawer : ContentAttrDrawer
    {
        protected override void DrawContent()
        {
            ItemDrawer.Value = EGUILayout.DrawEnumButton(Text, (Enum)ItemDrawer.Value, LayoutOptions);
        }

        protected override bool IsValidValueType()
        {
            return typeof(Enum).IsAssignableFrom(ItemDrawer.ValueType);
        }
    }
}
