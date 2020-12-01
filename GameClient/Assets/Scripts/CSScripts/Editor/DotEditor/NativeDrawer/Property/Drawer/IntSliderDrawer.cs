using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(IntSliderAttribute))]
    public class IntSliderDrawer : PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(int);
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<IntSliderAttribute>();

            int leftValue = attr.LeftValue;
            int rightValue = attr.RightValue;
            if (!string.IsNullOrEmpty(attr.LeftValueMemberName))
            {
                leftValue = DrawerUtility.GetMemberValue<int>(attr.LeftValueMemberName, Property.Target);
            }
            if (!string.IsNullOrEmpty(attr.RightValueMemberName))
            {
                rightValue = DrawerUtility.GetMemberValue<int>(attr.RightValueMemberName, Property.Target);
            }

            label = label ?? "";

            int value = Property.GetValue<int>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntSlider(label, value, leftValue, rightValue);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
