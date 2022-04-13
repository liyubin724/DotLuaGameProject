using UnityEditor;

namespace DotEditor.GUIExt.IMGUI
{
    public class IntSliderDrawer : ValueProviderLayoutDrawable<int>
    {
        public int MinValue { get; set; } = int.MinValue;
        public int MaxValue { get; set; } = int.MaxValue;

        protected override void OnLayoutDraw()
        {
            if (Value > MaxValue)
            {
                Value = MaxValue;
            }
            if (Value < MinValue)
            {
                Value = MinValue;
            }
            Value = EditorGUILayout.IntSlider(Label, Value, MinValue, MaxValue, LayoutOptions);
        }
    }
}
