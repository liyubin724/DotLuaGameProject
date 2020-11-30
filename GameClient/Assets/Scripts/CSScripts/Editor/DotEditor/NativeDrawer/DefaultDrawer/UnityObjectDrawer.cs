using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(UnityObject))]
    public class UnityObjectDrawer : Property.PropertyDrawer
    {
        protected override bool IsValidProperty()
        {
            return typeof(UnityObject).IsAssignableFrom(DrawerProperty.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            UnityObject value = DrawerProperty.GetValue<UnityObject>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.ObjectField(label, value, DrawerProperty.ValueType, true);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
