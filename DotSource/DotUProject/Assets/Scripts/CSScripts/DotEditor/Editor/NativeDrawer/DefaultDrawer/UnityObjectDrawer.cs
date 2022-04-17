using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(UnityObject))]
    public class UnityObjectDrawer : Property.PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return typeof(UnityObject).IsAssignableFrom(Property.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            UnityObject value = Property.GetValue<UnityObject>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.ObjectField(label, value, Property.ValueType, true);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
