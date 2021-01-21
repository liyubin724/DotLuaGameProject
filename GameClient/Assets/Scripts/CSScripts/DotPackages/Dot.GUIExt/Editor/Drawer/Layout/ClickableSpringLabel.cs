using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Layout
{
    public class ClickableSpringLabel : LayoutDrawable
    {
        public GUIStyle Style { get; set; } = EditorStyles.label;

        public Action OnClicked { get; set; } = null;
        
        protected override void OnLayoutDraw()
        {
            if (GUILayout.Button(Label, Style, LayoutOptions))
            {
                OnClicked?.Invoke();
            }
        }
    }
}
