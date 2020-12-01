using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(float))]
    public class FloatDrawer : Property.PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(float);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            float value = Property.GetValue<float>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.FloatField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
