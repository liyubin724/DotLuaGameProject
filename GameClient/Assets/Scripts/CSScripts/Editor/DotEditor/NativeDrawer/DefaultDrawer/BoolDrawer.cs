using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(bool))]
    public class BoolDrawer : Property.PropertyContentDrawer
    {
        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            bool value = Property.GetValue<bool>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Toggle(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(bool);
        }
    }
}
