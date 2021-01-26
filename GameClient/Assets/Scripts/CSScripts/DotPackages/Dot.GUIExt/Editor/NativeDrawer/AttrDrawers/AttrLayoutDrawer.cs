﻿using DotEngine.GUIExt.NativeDrawer;
using System;

namespace DotEditor.GUIExt.NativeDrawer
{
    public interface IAttrDrawer
    {
        DrawerAttribute Attr { get; set; }
        ItemDrawer ItemDrawer { get; set; }
        T GetAttr<T>() where T : DrawerAttribute;
    }

    public abstract class AttrDrawer : IAttrDrawer
    {
        public DrawerAttribute Attr { get; set; }
        public ItemDrawer ItemDrawer { get; set; }

        public T GetAttr<T>() where T : DrawerAttribute
        {
            return (T)Attr;
        }
    }

    public abstract class AttrLayoutDrawer : LayoutDrawer, IAttrDrawer
    {
        public DrawerAttribute Attr { get; set; }
        public ItemDrawer ItemDrawer { get; set; }

        public T GetAttr<T>() where T : DrawerAttribute
        {
            return (T)Attr;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomAttrDrawerAttribute : Attribute
    {
        public Type AttrType { get; private set; }

        public CustomAttrDrawerAttribute(Type attrType)
        {
            AttrType = attrType;
        }
    }
}