﻿using DotEngine.NativeDrawer.Layout;
using UnityEditor;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrDrawBinder(typeof(EndHorizontalAttribute))]
    public class EndHorizontalDrawer : LayoutDrawer
    {
        public EndHorizontalDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public override void OnGUILayout()
        {
            EditorGUILayout.EndHorizontal();
        }
    }
}
