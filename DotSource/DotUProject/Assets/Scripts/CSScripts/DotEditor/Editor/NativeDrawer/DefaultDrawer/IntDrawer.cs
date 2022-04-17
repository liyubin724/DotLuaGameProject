using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(int))]
    public class IntDrawer : Property.PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(int);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            int value = Property.GetValue<int>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntField(label, value);
            }
            if(EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
