﻿using UnityEditor;

namespace DotEditor.GUIExt.IMGUI
{
    public class ToolbarToggleDrawer : ValueProviderLayoutDrawable<bool>
    {
        public ToolbarToggleDrawer()
        {
            LabelWidth = 120;
        }

        protected override void OnLayoutDraw()
        {
            Value = EditorGUILayout.ToggleLeft(Label, Value,LayoutOptions);
        }
    }
}
