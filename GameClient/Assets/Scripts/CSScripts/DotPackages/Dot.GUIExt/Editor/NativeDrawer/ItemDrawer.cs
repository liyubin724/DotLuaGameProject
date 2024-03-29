﻿using DotEditor.GUIExt.IMGUI;
using DotEngine.GUIExt.NativeDrawer;
using DotEngine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class ItemDrawer : LayoutDrawer
    {
        public InstanceDrawer ParentDrawer { get; set; }
        public SystemObject Target { get; private set; }
        public FieldInfo Field { get; private set; }
        public int ItemIndex { get; private set; } = -1;

        public bool IsArrayItem
        {
            get
            {
                return Field == null && ItemIndex >= 0;
            }
        }

        public string Label
        {
            get
            {
                if (IsArrayItem)
                {
                    return Field != null ? $"{Field.Name}[{ItemIndex}]" : $"{ItemIndex}";
                }
                else
                {
                    return $"{Field.Name}";
                }
            }
        }

        public Type TargetType
        {
            get
            {
                return Target?.GetType();
            }
        }

        public Type ValueType
        {
            get
            {
                if (IsArrayItem)
                {
                    if(Value!=null)
                    {
                        return Value.GetType();
                    }else
                    {
                        return Field != null ? TypeUtility.GetElementTypeInArrayOrList(Field.FieldType) : TypeUtility.GetElementTypeInArrayOrList(Target.GetType());
                    }
                }
                else
                {
                    if(Value!=null)
                    {
                        return Value.GetType();
                    }else
                    {
                        return Field.FieldType;
                    }
                }
            }
        }

        public SystemObject Value
        {
            get
            {
                if (IsArrayItem)
                {
                    return ((IList)Target)[ItemIndex];
                }
                else
                {
                    return Field.GetValue(Target);
                }
            }
            set
            {
                if (IsArrayItem)
                {
                    ((IList)Target)[ItemIndex] = value;
                }
                else
                {
                    Field.SetValue(Target, value);
                }

                OnValueChanged?.Invoke(value);
            }
        }

        public event Action<object> OnValueChanged;

        private VisibleAttrDrawer visibleAttrDrawer = null;
        private List<LayoutAttrDrawer> layoutAttrDrawers = new List<LayoutAttrDrawer>();
        private List<DecoratorAttrDrawer> decoratorAttrDrawers = new List<DecoratorAttrDrawer>();

        private ILayoutDrawable innerDrawer = null;
        private bool isNeedRefresh = true;

        public ItemDrawer(SystemObject target, FieldInfo field)
        {
            Target = target;
            Field = field;

            InitAttrs();
        }

        public ItemDrawer(SystemObject target, int index)
        {
            Target = target;
            ItemIndex = index;
        }

        public void Refresh()
        {
            isNeedRefresh = true;
        }

        private void InitAttrs()
        {
            if(Field!=null)
            {
                VisibleAttribute visibleAttribute = Field.GetCustomAttribute<VisibleAttribute>();
                if(visibleAttribute!=null)
                {
                    visibleAttrDrawer = (VisibleAttrDrawer)DrawerUtility.GetAttrDrawerInstance(visibleAttribute);
                    visibleAttrDrawer.Attr = visibleAttribute;
                    visibleAttrDrawer.ItemDrawer = this;
                }

                decoratorAttrDrawers.Clear();
                DecoratorAttribute[] decoratorAttributes = (DecoratorAttribute[])Field.GetCustomAttributes(typeof(DecoratorAttribute), false);
                if(decoratorAttributes!=null && decoratorAttributes.Length>0)
                {
                    foreach(var attr in decoratorAttributes)
                    {
                        DecoratorAttrDrawer drawer = (DecoratorAttrDrawer)DrawerUtility.GetAttrDrawerInstance(attr);
                        drawer.ItemDrawer = this;
                        decoratorAttrDrawers.Add(drawer);
                    }
                }

                layoutAttrDrawers.Clear();
                LayoutAttribute[] layoutAttributes = (LayoutAttribute[])Field.GetCustomAttributes(typeof(LayoutAttribute), false);
                if(layoutAttributes!=null && layoutAttributes.Length>0)
                {
                    foreach(var attr in layoutAttributes)
                    {
                        LayoutAttrDrawer drawer = (LayoutAttrDrawer)DrawerUtility.GetAttrDrawerInstance(attr);
                        drawer.Occasion = attr.Occasion;
                        layoutAttrDrawers.Add(drawer);
                    }
                }

                ContentAttribute contentAttribute = Field.GetCustomAttribute<ContentAttribute>();
                if(contentAttribute!=null)
                {
                    ContentAttrDrawer contentAttrDrawer = (ContentAttrDrawer)DrawerUtility.GetAttrDrawerInstance(contentAttribute);
                    contentAttrDrawer.ItemDrawer = this;
                    innerDrawer = contentAttrDrawer;
                }
            }
        }

        private void RefreshDrawer()
        {
            if (innerDrawer != null)
            {
                return;
            }
            if (Target != null && Value != null)
            {
                TypeDrawer typeDrawer = DrawerUtility.GetTypeDrawerInstance(ValueType);
                if (typeDrawer != null)
                {
                    typeDrawer.Label = Label;
                    typeDrawer.ItemDrawer = this;
                    innerDrawer = typeDrawer;
                }
                else
                {
                    InstanceDrawer instanceDrawer = null;
                    if (TypeUtility.IsArrayOrListType(ValueType))
                    {
                        instanceDrawer = new ArrayDrawer(Value);
                    }
                    else if (TypeUtility.IsStructOrClassType(ValueType))
                    {
                        instanceDrawer = new ObjectDrawer(Value);
                    }

                    if (instanceDrawer != null)
                    {
                        instanceDrawer.Header = Label;
                        instanceDrawer.ItemDrawer = this;

                        instanceDrawer.IsShowInherit = ParentDrawer.IsShowInherit;
                        instanceDrawer.IsShowBox = ParentDrawer.IsShowBox;

                        innerDrawer = instanceDrawer;
                    }
                }
            }
        }

        public override void OnGUILayout()
        {
            if(!GetVisible())
            {
                return;
            }

            if(isNeedRefresh)
            {
                RefreshDrawer();
                isNeedRefresh = false;
            }

            foreach(var drawer in layoutAttrDrawers)
            {
                if (drawer.Occasion == LayoutOccasion.Before)
                {
                    drawer.OnGUILayout();
                }
            }

            foreach(var drawer in decoratorAttrDrawers)
            {
                drawer.OnGUILayout();
            }

            object value = Value;
            if (value == null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(Label, GUILayout.Width(EditorGUIUtility.labelWidth));
                    if (GUILayout.Button("Create"))
                    {
                        Value = DrawerUtility.GetTypeInstance(ValueType);

                        RefreshDrawer();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            else if (innerDrawer != null)
            {
                innerDrawer.OnGUILayout();
            }
            else
            {
                EGUI.BeginGUIColor(Color.red);
                {
                    EditorGUILayout.LabelField(Label, $"The drawer of {ValueType} is not found");
                }
                EGUI.EndGUIColor();
            }

            foreach (var drawer in layoutAttrDrawers)
            {
                if (drawer.Occasion == LayoutOccasion.After)
                {
                    drawer.OnGUILayout();
                }
            }
        }

        private bool GetVisible()
        {
            if(Field == null)
            {
                return true;
            }

            bool visible = Field.IsPublic;
            if(visibleAttrDrawer != null)
            {
                visible = visibleAttrDrawer.IsVisible();
            }
            return visible;
        }

    }
}