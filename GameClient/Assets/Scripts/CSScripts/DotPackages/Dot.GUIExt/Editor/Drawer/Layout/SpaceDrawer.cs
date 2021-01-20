﻿using UnityEditor;

namespace DotEditor.GUIExt.Layout
{
    public class SpaceDrawer : ILayoutDrawable
    {
        public float Width { get; set; } = 5.0f;
        public void OnGUILayout()
        {
            EditorGUILayout.Space(Width);
        }
    }
}
