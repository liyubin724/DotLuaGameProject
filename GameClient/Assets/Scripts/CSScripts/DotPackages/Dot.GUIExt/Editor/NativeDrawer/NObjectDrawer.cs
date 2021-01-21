using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class NObjectDrawer : NInstanceDrawer
    {
        private List<NLayoutDrawer> itemDrawers = new List<NLayoutDrawer>();

        public NObjectDrawer(SystemObject target) : base(target)
        {
        }

        protected override void RefreshDrawers()
        {
            itemDrawers.Clear();
            if(Target == null)
            {
                return;
            }

            Type[] allTypes = NDrawerUtility.GetAllBaseTypes(Target.GetType());
            if (allTypes != null && allTypes.Length > 0)
            {
                foreach (var type in allTypes)
                {
                    if (IsShowInherit)
                    {
                        itemDrawers.Add(new NHeadDrawer()
                        {
                            Header = type.Name,
                        });
                    }

                    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        if (NDrawerUtility.IsTypeSupported(field.FieldType))
                        {
                            NItemDrawer fieldDrawer = new NItemDrawer(Target, field);
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
                        itemDrawers.Add(new NHorizontalLineDrawer());
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