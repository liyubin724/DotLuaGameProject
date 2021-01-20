﻿using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Bounds))]
    public class BoundsTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.BoundsField(Label, (Bounds)ItemDrawer.Value);
        }
    }
}