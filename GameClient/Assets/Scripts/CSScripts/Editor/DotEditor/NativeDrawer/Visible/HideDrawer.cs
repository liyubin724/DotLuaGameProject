﻿using DotEngine.NativeDrawer.Visible;

namespace DotEditor.NativeDrawer.Visible
{
    [CustomAttributeDrawer(typeof(HideAttribute))]
    public class HideDrawer : VisibleDrawer
    {
        public HideDrawer(VisibleAtrribute attr) : base(attr)
        {
        }

        public override bool IsVisible()
        {
            return false;
        }
    }
}
