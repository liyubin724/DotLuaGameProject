﻿using DotEditor.GUIExt.IMGUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class ObjectDrawer : InstanceDrawer
    {
        private List<ILayoutDrawable> itemDrawers = new List<ILayoutDrawable>();

        public ObjectDrawer(SystemObject target) : base(target)
        {
        }

        protected override void RefreshDrawers()
        {
            itemDrawers.Clear();
            if(Target == null)
            {
                return;
            }

            Type[] allTypes = DrawerUtility.GetAllBaseTypes(Target.GetType());
            if (allTypes != null && allTypes.Length > 0)
            {
                if(!IsShowInherit)
                {
                    itemDrawers.Add(new HeaderDrawer()
                    {
                        Header = allTypes[allTypes.Length-1].Name,
                    });
                }
                foreach (var type in allTypes)
                {
                    if (IsShowInherit)
                    {
                        itemDrawers.Add(new HeaderDrawer()
                        {
                            Header = type.Name,
                        });
                    }

                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        if (DrawerUtility.IsTypeSupported(field.FieldType))
                        {
                            ItemDrawer fieldDrawer = new ItemDrawer(Target, field);
                            fieldDrawer.ParentDrawer = this;

                            itemDrawers.Add(fieldDrawer);
                        }
                        else
                        {
                            itemDrawers.Add(new UnsupportedTypeDrawer()
                            {
                                Label = field.Name,
                                TargetType = field.FieldType,
                            });
                        }
                    }
                    if (IsShowInherit)
                    {
                        itemDrawers.Add(new HorizontalLineDrawer());
                    }
                }
            }
        }

        protected override void DrawInstance()
        {
            if(!string.IsNullOrEmpty(Header))
            {
                EditorGUILayout.LabelField(Header);
            }
            if(itemDrawers.Count>0)
            {
                EGUI.BeginIndent();
                {
                    foreach (var drawer in itemDrawers)
                    {
                        drawer.OnGUILayout();
                    }
                }
                EGUI.EndIndent();
            }
        }
    }
}