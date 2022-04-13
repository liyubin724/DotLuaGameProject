using System;
using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class EnumTypeDrawer : TypeDrawer
    {
        public override void OnGUILayout()
        {
            Type valueType = ItemDrawer.ValueType;
            var attrs = valueType.GetCustomAttributes(typeof(FlagsAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                ItemDrawer.Value = EditorGUILayout.EnumFlagsField(Label, (Enum)ItemDrawer.Value);
            }
            else
            {
                ItemDrawer.Value = EditorGUILayout.EnumPopup(Label, (Enum)ItemDrawer.Value);
            }
        }
    }
}