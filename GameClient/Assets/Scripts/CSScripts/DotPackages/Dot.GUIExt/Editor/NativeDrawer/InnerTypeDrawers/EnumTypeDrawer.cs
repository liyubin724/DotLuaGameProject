using System;
using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class EnumTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            Type valueType = FieldDrawer.ValueType;
            var attrs = valueType.GetCustomAttributes(typeof(FlagsAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                FieldDrawer.Value = EditorGUILayout.EnumFlagsField(Label, (Enum)FieldDrawer.Value);
            }
            else
            {
                FieldDrawer.Value = EditorGUILayout.EnumPopup(Label, (Enum)FieldDrawer.Value);
            }
        }
    }
}