using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class FloatSliderAttrDrawer : ContentAttrDrawer
    {
        protected override void DrawContent()
        {
            FloatSliderAttribute attr = GetAttr<FloatSliderAttribute>();

            float leftValue = attr.LeftValue;
            float rightValue = attr.RightValue;
            if (!string.IsNullOrEmpty(attr.LeftValueMemberName))
            {
                leftValue = NDrawerUtility.GetMemberValue<float>(attr.LeftValueMemberName, ItemDrawer.Target);
            }

            if (!string.IsNullOrEmpty(attr.RightValueMemberName))
            {
                rightValue = NDrawerUtility.GetMemberValue<float>(attr.RightValueMemberName, ItemDrawer.Target);
            }

            float value = (float)ItemDrawer.Value;
            if (value < leftValue)
            {
                value = leftValue;
                ItemDrawer.Value = value;
            }
            if (value > rightValue)
            {
                value = rightValue;
                ItemDrawer.Value = value;
            }

            ItemDrawer.Value = EditorGUILayout.Slider(Label, value, leftValue, rightValue);
        }

        protected override bool IsValidValueType()
        {
            return ItemDrawer.ValueType == typeof(float);
        }
    }
}
