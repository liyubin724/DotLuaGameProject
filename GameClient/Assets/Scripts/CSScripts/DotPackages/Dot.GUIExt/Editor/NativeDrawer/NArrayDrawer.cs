using DotEngine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class NArrayDrawer : NInstanceDrawer
    {
        public Action CreateNewItem = null;
        public Action ClearAllItem = null;
        public Action<int> DeleteItemAt = null;
        private List<NLayoutDrawer> itemDrawers = new List<NLayoutDrawer>();

        private Type ItemType
        {
            get
            {
                if(Target!=null && Target is IList list)
                {
                    return TypeUtility.GetElementTypeInArrayOrList(list.GetType());
                }
                return null;
            }
        }

        public NArrayDrawer(SystemObject target) : base(target)
        {
        }

        public NArrayDrawer(NItemDrawer itemDrawer) : this(itemDrawer.Value)
        {
        }

        protected override void RefreshDrawers()
        {
            itemDrawers.Clear();
            if(Target!=null && Target is IList list)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    Type itemType = list[i].GetType();
                    bool isTypeSupported = NDrawerUtility.IsTypeSupported(itemType);

                    if (!isTypeSupported)
                    {
                        itemDrawers.Add(new UnsupportedTypeDrawer()
                        {
                            Label = "" + i,
                            TargetType = itemType
                        });
                    }
                    else
                    {
                        NItemDrawer itemDrawer = new NItemDrawer(Target, i);
                        itemDrawer.ParentDrawer = this;

                        itemDrawers.Add(itemDrawer);
                    }
                }
            }
        }

        private void Clear()
        {
            itemDrawers.Clear();
            if (Target != null && Target is IList list)
            {
                list.Clear();
            }

            Refresh();
        }

        private void AddNewItemAtLast()
        {
            if (Target != null && Target is IList list)
            {
                SystemObject item = NDrawerUtility.GetTypeInstance(ItemType);
                if(item!=null)
                {
                    if (TypeUtility.IsArrayType(list.GetType()))
                    {
                        Array array = (Array)list;
                        DotEngine.Utilities.ArrayUtility.Add(ref array, item);

                        Target = array;
                    }
                    else
                    {
                        list.Add(item);
                    }
                }

                Refresh();
            }
        }

        private void RemoveItemAtIndex(int index)
        {
            if (Target != null && Target is IList list)
            {
                if (TypeUtility.IsArrayType(list.GetType()))
                {
                    Array array = (Array)list;
                    DotEngine.Utilities.ArrayUtility.Remove(ref array, index);

                    Target = array;
                }
                else
                {
                    list.RemoveAt(index);
                }

                Refresh();
            }
        }

        protected override void DrawInstance()
        {
            EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
            {
                EGUILayout.DrawBoxHeader(Header, GUILayout.ExpandWidth(true));

                Rect lastRect = GUILayoutUtility.GetLastRect();
                Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y, 40, lastRect.height);
                if (GUI.Button(clearBtnRect, "C", EditorStyles.toolbarButton))
                {
                    if(ClearAllItem!=null)
                    {
                        ClearAllItem.Invoke();
                    }else
                    {
                        Clear();
                    }
                }

                IList list = Target as IList;

                for (int i = 0; i < list.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            itemDrawers[i].OnGUILayout();
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(GUILayout.Width(20));
                        {
                            if (GUILayout.Button("-"))
                            {
                                if(DeleteItemAt!=null)
                                {
                                    DeleteItemAt(i);
                                }else
                                {
                                    RemoveItemAtIndex(i);
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();

                    EGUILayout.DrawHorizontalLine();
                }

                Rect addBtnRect = GUILayoutUtility.GetRect(lastRect.width, 20);
                addBtnRect.x += addBtnRect.width - 40;
                addBtnRect.width = 40;
                if (GUI.Button(addBtnRect, "+"))
                {
                    if(CreateNewItem!=null)
                    {
                        CreateNewItem.Invoke();
                    }else
                    {
                        AddNewItemAtLast();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}