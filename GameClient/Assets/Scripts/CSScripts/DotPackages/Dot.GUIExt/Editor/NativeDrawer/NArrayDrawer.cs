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
        private NItemDrawer itemDrawer = null;
        private List<NLayoutDrawer> itemDrawers = new List<NLayoutDrawer>();
        private IList list = null;

        public NArrayDrawer(SystemObject target) : base(target)
        {
            list = target as IList;
        }

        public NArrayDrawer(NItemDrawer itemDrawer) : this(itemDrawer.Value)
        {
        }

        protected override void InitDrawers()
        {
            itemDrawers.Clear();
            if (list != null)
            {
                Type itemType = TypeUtility.GetElementTypeInArrayOrList(list.GetType());
                bool isTypeSupported = NDrawerUtility.IsTypeSupported(itemType);
                for (int i = 0; i < list.Count; ++i)
                {
                    if (isTypeSupported)
                    {
                        itemDrawers.Add(new UnsupportedTypeDrawer()
                        {
                            Label = "" + i,
                            TargetType = itemType
                        });
                    }
                    else
                    {
                        itemDrawers.Add(new NItemDrawer(Target, i));
                    }
                }
            }
        }

        private void Clear()
        {
            itemDrawers.Clear();
            list.Clear();
        }

        private void AddNewItemAtLast()
        {
            SystemObject item = NDrawerUtility.GetTypeInstance(TypeUtility.GetElementTypeInArrayOrList(list.GetType()));
            if (TypeUtility.IsArrayType(list.GetType()))
            {
                Array array = (Array)list;
                DotEngine.Utilities.ArrayUtility.Add(ref array, item);
                list = array;

                if (itemDrawer != null)
                {
                    itemDrawer.Value = list;
                }
            }
            else
            {
                list.Add(item);
            }
        }

        private void RemoveItemAtIndex(int index)
        {
            if (TypeUtility.IsArrayType(list.GetType()))
            {
                Array array = (Array)list;
                DotEngine.Utilities.ArrayUtility.Remove(ref array, index);
                list = array;

                if (itemDrawer != null)
                {
                    itemDrawer.Value = list;
                }
            }
            else
            {
                list.RemoveAt(index);
            }
        }

        protected override void DrawInstance()
        {
            EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
            {
                EditorGUILayout.LabelField(GUIContent.none, EditorStyles.toolbar, GUILayout.ExpandWidth(true));

                Rect lastRect = GUILayoutUtility.GetLastRect();
                EditorGUI.LabelField(lastRect, Header, EGUIStyles.BoldLabelStyle);

                Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y, 40, lastRect.height);
                if (GUI.Button(clearBtnRect, "C", EditorStyles.toolbarButton))
                {
                    Clear();
                }

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
                                RemoveItemAtIndex(i);
                                Refresh();
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
                    AddNewItemAtLast();
                    Refresh();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}