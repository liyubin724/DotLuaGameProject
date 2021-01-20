using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Layout
{
    public class ToolbarButtonDrawer : LayoutDrawable
    {
        public float Width { get; set; } = 60.0f;
        public Action OnClicked { get; set; } = null;

        protected override void OnLayoutDraw()
        {
            if (GUILayout.Button(Label, EditorStyles.toolbarButton, GUILayout.Width(Width)))
            {
                OnClicked?.Invoke();
            }
        }
    }
}