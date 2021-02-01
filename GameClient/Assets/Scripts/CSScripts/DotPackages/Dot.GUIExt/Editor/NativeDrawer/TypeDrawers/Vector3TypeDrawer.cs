﻿using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Vector3))]
    public class Vector3TypeDrawer : TypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.Vector3Field(Label, (Vector3)ItemDrawer.Value);
        }
    }
}
