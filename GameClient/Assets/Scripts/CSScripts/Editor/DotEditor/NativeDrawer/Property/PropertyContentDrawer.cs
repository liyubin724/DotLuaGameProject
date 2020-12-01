﻿using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Property
{
    public abstract class PropertyDrawer : Drawer
    {

    }

    public abstract class PropertyControlDrawer : PropertyDrawer
    {
        public abstract void OnStartGUILayout();
        public abstract void OnEndGUILayout();
    }

    public abstract class PropertyLabelDrawer : PropertyDrawer
    {
        public abstract string GetLabel();
    }

    public abstract class PropertyContentDrawer : PropertyDrawer
    {
        protected abstract bool IsValidProperty();

        public void OnGUILayout(string label)
        {
            if(!IsValidProperty())
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
