﻿using DotEngine.NativeDrawer.Layout;
using DotEditor.GUIExtension;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrBinder(typeof(EndIndentAttribute))]
    public class EndIndentDrawer : LayoutDrawer
    {
        public EndIndentDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public override void OnGUILayout()
        {
            EGUI.EndIndent();
        }
    }
}
