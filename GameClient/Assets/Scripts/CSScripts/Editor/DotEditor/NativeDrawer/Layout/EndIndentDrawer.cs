﻿using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Layout;

namespace DotEditor.NativeDrawer.Layout
{
    [Binder(typeof(EndIndentAttribute))]
    public class EndIndentDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            EGUI.EndIndent();
        }
    }
}
