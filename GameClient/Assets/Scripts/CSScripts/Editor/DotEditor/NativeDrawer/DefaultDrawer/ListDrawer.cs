﻿using DotEngine.Utilities;
using DotEditor.GUIExtension;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(IList))]
    public class ListDrawer : Property.PropertyContentDrawer
    {
        private IList list = null;
        private List<DrawerProperty> elementProperties = new List<DrawerProperty>();
        public ListDrawer()
        {
            InitList();
        }

        private void InitList()
        {
            list = Property.GetValue<IList>();
            if(list!=null)
            {
                elementProperties.Clear();
                for (int i = 0; i < list.Count; ++i)
                {
                    elementProperties.Add(new DrawerProperty(Property.Target,Property.Field,i));
                }
            }
        }

        protected override bool IsValidProperty()
        {
            return TypeUtility.IsArrayOrList(Property.ValueType);
        }

        protected override void OnDrawProperty(string label)
        {
            if(list == null)
            {
                list = Property.GetValue<IList>();
            }

            EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
            {
                EditorGUILayout.LabelField(GUIContent.none, EditorStyles.toolbar, GUILayout.ExpandWidth(true));

                Rect lastRect = GUILayoutUtility.GetLastRect();
                EditorGUI.LabelField(lastRect, label, EGUIStyles.BoldLabelStyle);

                Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y, 40, lastRect.height);
                if (GUI.Button(clearBtnRect, "C",EditorStyles.toolbarButton))
                {
                    Property.ClearArrayElement();
                    InitList();
                }

                for (int i = 0; i < list.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical();
                        {
                            elementProperties[i].OnGUILayout();
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical(GUILayout.Width(20));
                        {
                            if (GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                Property.RemoveArrayElementAtIndex(i);
                                InitList();
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
                    Property.AddArrayElement();
                    InitList();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}
