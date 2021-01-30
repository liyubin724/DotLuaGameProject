using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(MultilineTextAttribute))]
    public class MultilineTextDrawer : PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            var attr = GetAttr<MultilineTextAttribute>();

            label = label ?? "";
            string value = Property.GetValue<string>();

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
                Property.Value = value;
            }
        }
    }
}
