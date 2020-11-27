﻿using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Vector3))]
    public class DefaultVector3Drawer : NativeTypeDrawer
    {
        public DefaultVector3Drawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(Vector3);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Vector3 value = DrawerProperty.GetValue<Vector3>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Vector3Field(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
