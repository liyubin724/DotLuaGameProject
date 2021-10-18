using System;
using UnityEngine;

namespace DotEditor.GUIExt.IMGUI
{
    public class ButtonDrawer : LayoutDrawable
    {
        public Action OnClicked { get; set; } = null;
        public GUIStyle Style { get; set; } = GUIStyle.none;

        protected override void OnLayoutDraw()
        {
            if (GUILayout.Button(Label, Style, LayoutOptions))
            {
                OnClicked?.Invoke();
            }
        }
    }
}
