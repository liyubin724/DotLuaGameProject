﻿using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer
{
    public abstract class NativeTypeDrawer
    {
        public NativeDrawerProperty DrawerProperty { get; private set; }

        protected NativeTypeDrawer(NativeDrawerProperty property)
        {
            DrawerProperty = property;
        }

        protected abstract bool IsValidProperty();

        public void OnGUILayout(string label)
        {
            if(DrawerProperty == null || !IsValidProperty())
            {
                OnDrawInvalidProperty(label);
            }else
            {
                OnDrawProperty(label);
            }
        }

        protected abstract void OnDrawProperty(string label);

        protected virtual void OnDrawInvalidProperty(string label)
        {
            EGUI.BeginGUIColor(Color.red);
            {
                EditorGUILayout.LabelField(string.IsNullOrEmpty(label) ? "" : label, "Invalid");
            }
            EGUI.EndGUIColor();
        }

    }
}
