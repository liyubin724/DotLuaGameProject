using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class EnumTypeDrawer : NativeTypeDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, NativeFieldDrawer field)
        {
            Type valueType = field.ValueType;
            var attrs = valueType.GetCustomAttributes(typeof(FlagsAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                field.Value = EditorGUI.EnumFlagsField(rect, label, (Enum)field.Value);
            }
            else
            {
                field.Value = EditorGUI.EnumPopup(rect, label, (Enum)field.Value);
            }
        }
    }
}
