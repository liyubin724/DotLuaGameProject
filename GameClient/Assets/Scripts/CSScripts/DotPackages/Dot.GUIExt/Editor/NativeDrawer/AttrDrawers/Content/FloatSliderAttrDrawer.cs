using DotEditor.GUIExt.Layout;
using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class FloatSliderAttrDrawer : ContentAttrDrawer
    {
        private FloatSliderDrawer drawer = null;

        protected override void DrawContent()
        {
            if(drawer == null)
            {
                drawer = new FloatSliderDrawer();
                drawer.OnValueChanged = (value) =>
                {
                    ItemDrawer.Value = value;
                };
                drawer.Value = (float)ItemDrawer.Value;
            }
            FloatSliderAttribute attr = GetAttr<FloatSliderAttribute>();

            float leftValue = attr.MinValue;
            float rightValue = attr.MaxValue;
            if (!string.IsNullOrEmpty(attr.MinValueMemberName))
            {
                leftValue = NDrawerUtility.GetMemberValue<float>(attr.MinValueMemberName, ItemDrawer.Target);
            }
            if (!string.IsNullOrEmpty(attr.MaxValueMemberName))
            {
                rightValue = NDrawerUtility.GetMemberValue<float>(attr.MaxValueMemberName, ItemDrawer.Target);
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
