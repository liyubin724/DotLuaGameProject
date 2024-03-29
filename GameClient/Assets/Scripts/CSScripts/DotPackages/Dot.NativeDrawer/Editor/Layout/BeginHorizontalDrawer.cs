﻿using DotEngine.NativeDrawer.Layout;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Layout
{
    [Binder(typeof(BeginHorizontalAttribute))]
    public class BeginHorizontalDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        }
    }
}
