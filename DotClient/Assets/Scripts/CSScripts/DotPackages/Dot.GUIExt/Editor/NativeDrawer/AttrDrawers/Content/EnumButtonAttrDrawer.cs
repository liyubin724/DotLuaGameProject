using DotEditor.GUIExt.IMGUI;
using DotEngine.GUIExt.NativeDrawer;
using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(EnumButtonAttribute))]
    public class EnumButtonAttrDrawer : ContentAttrDrawer
    {
        private EnumButtonDrawer drawer = null;

        public EnumButtonAttrDrawer()
        {
        }

        protected override void DrawContent()
        {
            if(drawer == null)
            {
                EnumButtonAttribute attr = GetAttr<EnumButtonAttribute>();
                drawer = new EnumButtonDrawer(ItemDrawer.ValueType)
                {
                    MinWidth = attr.MinWidth,
                    MaxWidth = attr.MaxWidth,
                    Text = ItemDrawer.Label,
                    IsExpandWidth = attr.IsExpandWidth,
                    Value = (Enum)ItemDrawer.Value,
                    OnValueChanged = (value) =>
                    {
                        ItemDrawer.Value = value;
                    }
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
