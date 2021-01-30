using UnityEditor;

namespace DotEditor.GUIExt.IMGUI
{
    public class FloatSliderDrawer : ValueProviderLayoutDrawable<float>
    {
        public float MinValue { get; set; } = float.MinValue;
        public float MaxValue { get; set; } = float.MaxValue;
        protected override void OnLayoutDraw()
        {
            if(Value>MaxValue)
            {
                Value = MaxValue;
            }
            if(Value<MinValue)
            {
                Value = MinValue;
            }
            Value = EditorGUILayout.Slider(Label, Value, MinValue, MaxValue, LayoutOptions);
        }
    }
}
