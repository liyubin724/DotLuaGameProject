using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(string))]
    public class StringDrawer : Property.PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            string value = Property.GetValue<string>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.TextField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
