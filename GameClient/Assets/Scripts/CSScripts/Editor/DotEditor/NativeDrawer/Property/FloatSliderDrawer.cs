using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [AttrDrawBinder(typeof(FloatSliderAttribute))]
    public class FloatSliderDrawer : PropertyDrawer
    {
        public FloatSliderDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override void OnDrawProperty(string label)
        {
            FloatSliderAttribute attr = GetAttr<FloatSliderAttribute>();

            float leftValue = attr.LeftValue;
            float rightValue = attr.RightValue;
            if(!string.IsNullOrEmpty(attr.LeftValueMemberName))
            {
                leftValue = NativeDrawerUtility.GetMemberValue<float>(attr.LeftValueMemberName, DrawerProperty.Target);
            }
            if(!string.IsNullOrEmpty(attr.RightValueMemberName))
            {
                rightValue = NativeDrawerUtility.GetMemberValue<float>(attr.RightValueMemberName, DrawerProperty.Target);
            }

            label = label ?? "";

            float value = DrawerProperty.GetValue<float>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Slider(label, value, leftValue, rightValue);
            }
            if(EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(float);
        }
    }
}
