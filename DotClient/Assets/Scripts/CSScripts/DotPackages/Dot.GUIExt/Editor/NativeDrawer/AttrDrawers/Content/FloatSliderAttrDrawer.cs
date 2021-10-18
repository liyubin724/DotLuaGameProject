using DotEditor.GUIExt.IMGUI;
using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(FloatSliderAttribute))]
    public class FloatSliderAttrDrawer : ContentAttrDrawer
    {
        private FloatSliderDrawer drawer = null;

        protected override void DrawContent()
        {
            FloatSliderAttribute attr = GetAttr<FloatSliderAttribute>();
            if(drawer == null)
            {
                drawer = new FloatSliderDrawer();
                drawer.IsExpandWidth = attr.IsExpandWidth;
                drawer.Text = ItemDrawer.Label;
                drawer.OnValueChanged = (value) =>
                {
                    ItemDrawer.Value = value;
                };
                drawer.Value = (float)ItemDrawer.Value;
            }

            float leftValue = attr.MinValue;
            float rightValue = attr.MaxValue;
            if (!string.IsNullOrEmpty(attr.MinValueMemberName))
            {
                leftValue = DrawerUtility.GetMemberValue<float>(attr.MinValueMemberName, ItemDrawer.Target);
            }
            if (!string.IsNullOrEmpty(attr.MaxValueMemberName))
            {
                rightValue = DrawerUtility.GetMemberValue<float>(attr.MaxValueMemberName, ItemDrawer.Target);
            }
            drawer.MinValue = leftValue;
            drawer.MaxValue = rightValue;
            drawer.OnGUILayout();
        }

        protected override bool IsValidValueType()
        {
            return ItemDrawer.ValueType == typeof(float);
        }
    }
}
