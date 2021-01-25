using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.IMGUI
{
    public class ToolbarButtonDrawer : LayoutDrawable
    {
        public Action OnClicked { get; set; } = null;

        public ToolbarButtonDrawer()
        {
            Width = 60.0f;
        }

        protected override void OnLayoutDraw()
        {
            if (GUILayout.Button(Label, EditorStyles.toolbarButton, LayoutOptions))
            {
                OnClicked?.Invoke();
            }
        }
    }
}