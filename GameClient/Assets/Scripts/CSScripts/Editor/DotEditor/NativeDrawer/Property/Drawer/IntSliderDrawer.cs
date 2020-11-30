using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [AttrBinder(typeof(IntSliderAttribute))]
    public class IntSliderDrawer : PropertyDrawer
    {
        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(int);
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<IntSliderAttribute>();

            int leftValue = attr.LeftValue;
            int rightValue = attr.RightValue;
            if (!string.IsNullOrEmpty(attr.LeftValueMemberName))
            {
                leftValue = DrawerUtility.GetMemberValue<int>(attr.LeftValueMemberName, DrawerProperty.Target);
            }
            if (!string.IsNullOrEmpty(attr.RightValueMemberName))
            {
                rightValue = DrawerUtility.GetMemberValue<int>(attr.RightValueMemberName, DrawerProperty.Target);
            }

            label = label ?? "";

            int value = DrawerProperty.GetValue<int>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntSlider(label, value, leftValue, rightValue);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
