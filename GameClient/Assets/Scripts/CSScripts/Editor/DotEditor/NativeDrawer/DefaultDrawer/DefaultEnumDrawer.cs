using System;
using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Enum))]
    public class DefaultEnumDrawer : TypeDrawer
    {
        public DefaultEnumDrawer(DrawerProperty property) : base(property)
        {
            
        }

        protected override void OnDrawProperty(string label)
        {
            var flagAttrs = DrawerProperty.ValueType.GetCustomAttributes(typeof(FlagsAttribute), false);
            bool isFlagEnum = false;
            if (flagAttrs != null && flagAttrs.Length > 0)
            {
                isFlagEnum = true;
            }

            label = label ?? "";
            Enum value = DrawerProperty.GetValue<Enum>();
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
                DrawerProperty.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return typeof(Enum).IsAssignableFrom(DrawerProperty.ValueType);
        }
    }
}
