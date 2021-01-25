using DotEditor.GUIExt.IMGUI;
using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class EnumButtonAttrDrawer : ContentAttrDrawer
    {
        private EnumButtonDrawer drawer = null;
        protected override void DrawContent()
        {
            if(drawer == null)
            {
                drawer = new EnumButtonDrawer(ItemDrawer.ValueType);
                drawer.Value = (Enum)ItemDrawer.Value;
                drawer.OnValueChanged = (value) =>
                {
                    ItemDrawer.Value = value;
                };
            }
            drawer.OnGUILayout();
        }

        protected override bool IsValidValueType()
        {
            return typeof(Enum).IsAssignableFrom(ItemDrawer.ValueType);
        }
    }
}
