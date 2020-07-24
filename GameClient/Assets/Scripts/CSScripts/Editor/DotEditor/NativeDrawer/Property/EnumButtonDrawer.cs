using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [CustomAttributeDrawer(typeof(EnumButtonAttribute))]
    public class EnumButtonDrawer : PropertyDrawer
    {
        public EnumButtonDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return typeof(Enum).IsAssignableFrom(DrawerProperty.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";

            object value = DrawerProperty.Value;
            EditorGUI.BeginChangeCheck();
            {
                value = EGUILayout.DrawEnumButton(label, (Enum)value, GetLayoutOptions());
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = Enum.ToObject(DrawerProperty.ValueType, value);
            }
        }

        private GUILayoutOption[] options = null;
        private GUILayoutOption[] GetLayoutOptions()
        {
            if(options == null)
            {
                var attr = GetAttr<EnumButtonAttribute>();

                List<GUILayoutOption> oList = new List<GUILayoutOption>();
                if(attr.MaxWidth>0)
                {
                    oList.Add(GUILayout.MaxWidth(attr.MaxWidth));
                }
                if(attr.MinWidth>0)
                {
                    oList.Add(GUILayout.MinWidth(attr.MinWidth));
                }

                options = oList.ToArray();
            }

            return options;
        }
    }
}
