using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [AttrBinder(typeof(MultilineTextAttribute))]
    public class MultilineTextDrawer : PropertyDrawer
    {
        public MultilineTextDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<MultilineTextAttribute>();

            label = label ?? "";
            string value = DrawerProperty.GetValue<string>();

            EditorGUILayout.LabelField(label);
            EditorGUI.BeginChangeCheck();
            {
                EGUI.BeginIndent();
                {
                    value = EditorGUILayout.TextArea(value,GUILayout.Height(EditorGUIUtility.singleLineHeight * attr.LineCount));
                }
                EGUI.EndIndent();
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
