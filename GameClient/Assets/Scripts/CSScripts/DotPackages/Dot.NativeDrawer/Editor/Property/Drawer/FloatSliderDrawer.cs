using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(FloatSliderAttribute))]
    public class FloatSliderDrawer : PropertyContentDrawer
    {
        protected override void OnDrawProperty(string label)
        {
            FloatSliderAttribute attr = GetAttr<FloatSliderAttribute>();

            float leftValue = attr.LeftValue;
            float rightValue = attr.RightValue;
            if(!string.IsNullOrEmpty(attr.LeftValueMemberName))
            {
                leftValue = DrawerUtility.GetMemberValue<float>(attr.LeftValueMemberName, Property.Target);
            }

            if(!string.IsNullOrEmpty(attr.RightValueMemberName))
            {
                rightValue = DrawerUtility.GetMemberValue<float>(attr.RightValueMemberName, Property.Target);
            }

            label = label ?? "";

            float value = Property.GetValue<float>();
            if (value < leftValue)
            {
                value = leftValue;
                Property.Value = value;
            }
            if (value > rightValue)
            {
                value = rightValue;
                Property.Value = value;
            }

            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Slider(label, value, leftValue, rightValue);
            }
            if(EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(float);
        }
    }
}
