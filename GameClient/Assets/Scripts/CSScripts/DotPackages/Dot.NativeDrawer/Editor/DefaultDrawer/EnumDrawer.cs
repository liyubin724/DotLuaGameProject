using System;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class EnumDrawer : Property.PropertyContentDrawer
    {
        protected override void OnDrawProperty(string label)
        {
            var flagAttrs = Property.ValueType.GetCustomAttributes(typeof(FlagsAttribute), false);
            bool isFlagEnum = false;
            if (flagAttrs != null && flagAttrs.Length > 0)
            {
                isFlagEnum = true;
            }

            label = label ?? "";
            Enum value = Property.GetValue<Enum>();
            EditorGUI.BeginChangeCheck();
            {
                if(isFlagEnum)
                {
                    value = EditorGUILayout.EnumFlagsField(label, value);
                }else
                {
                    value = EditorGUILayout.EnumPopup(label, value);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return typeof(Enum).IsAssignableFrom(Property.ValueType);
        }
    }
}
