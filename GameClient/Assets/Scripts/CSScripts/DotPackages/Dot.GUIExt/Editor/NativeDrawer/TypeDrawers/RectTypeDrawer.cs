﻿using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Rect))]
    public class RectTypeDrawer : TypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.RectField(Label, (Rect)ItemDrawer.Value);
        }
    }
}