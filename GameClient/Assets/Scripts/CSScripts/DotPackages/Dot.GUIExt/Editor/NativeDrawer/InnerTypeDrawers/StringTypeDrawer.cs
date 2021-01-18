﻿using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [NativeTypeDrawer(typeof(string))]
    public class StringTypeDrawer : NativeTypeDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, NativeField field)
        {
            field.Value = EditorGUI.TextField(rect, label, (string)field.Value);
        }
    }
}