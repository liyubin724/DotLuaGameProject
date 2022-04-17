using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Property;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(EnumButtonAttribute))]
    public class EnumButtonDrawer : PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return typeof(Enum).IsAssignableFrom(Property.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";

            object value = Property.Value;
            EditorGUI.BeginChangeCheck();
            {
                value = EGUILayout.DrawEnumButton(label, (Enum)value, GetLayoutOptions());
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = Enum.ToObject(Property.ValueType, value);
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
