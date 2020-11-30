﻿using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Decorator;
using UnityEngine;

namespace DotEditor.NativeDrawer.Decorator
{
    [AttrBinder(typeof(BoxedHeaderAttribute))]
    public class BoxedHeaderDrawer : DecoratorDrawer
    {
        public override void OnGUILayout()
        {
            BoxedHeaderAttribute attr = GetAttr<BoxedHeaderAttribute>();
            EGUILayout.DrawBoxHeader(attr.Header, GUILayout.ExpandWidth(true));
        }
    }
}
