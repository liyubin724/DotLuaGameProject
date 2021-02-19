using DotEditor.GUIExt.IMGUI;
using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(MultilineTextAttribute))]
    public class MultilineTextAttrDrawer : ContentAttrDrawer
    {
        private MultilineTextDrawer drawer = null;
        protected override void DrawContent()
        {
            if(drawer ==null)
            {
                MultilineTextAttribute attr = GetAttr<MultilineTextAttribute>();
                drawer = new MultilineTextDrawer();
                drawer.IsExpandWidth = true;
                drawer.LineCount = attr.LineCount;
                drawer.Text = ItemDrawer.Label;
                drawer.OnValueChanged = (value) =>
                {
                    ItemDrawer.Value = value;
                };
                drawer.Value = (string)ItemDrawer.Value;
            }
            drawer.OnGUILayout();
        }

        protected override bool IsValidValueType()
        {
            return ItemDrawer.ValueType == typeof(string);
        }
    }
}
