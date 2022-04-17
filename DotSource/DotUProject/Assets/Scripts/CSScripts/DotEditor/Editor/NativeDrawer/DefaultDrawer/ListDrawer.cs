using DotEditor.GUIExtension;
using DotEngine.Core.Extensions;
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
        private bool IsNeedRefresh = true;
        private List<DrawerProperty> elementProperties = new List<DrawerProperty>();

        protected override bool IsValidProperty()
        {
            return Property.ValueType.IsArrayOrListType();
        }

        protected override void OnDrawProperty(string label)
        {
            if(IsNeedRefresh)
            {
                InitList();
            }

            if(list == null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel(label);
                    if (GUILayout.Button("Create"))
                    {
                        Property.Value = DrawerUtility.CreateInstance(Property.ValueType);

                        IsNeedRefresh = true;
                    }
                }
                EditorGUILayout.EndHorizontal();
                return;
            }else
            {
                EditorGUILayout.BeginVertical(EGUIStyles.BoxStyle);
                {
                    EditorGUILayout.LabelField(GUIContent.none, EditorStyles.toolbar, GUILayout.ExpandWidth(true));

                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    EditorGUI.LabelField(lastRect, label, EGUIStyles.BoldLabelStyle);

                    Rect clearBtnRect = new Rect(lastRect.x + lastRect.width - 40, lastRect.y, 40, lastRect.height);
                    if (GUI.Button(clearBtnRect, "C", EditorStyles.toolbarButton))
                    {
                        Property.ClearArrayElement();
                        IsNeedRefresh = true;
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
                                if (GUILayout.Button("-"))
                                {
                                    Property.RemoveArrayElementAtIndex(i);
                                    IsNeedRefresh = true;
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
                        IsNeedRefresh = true;
                    }
                }
                EditorGUILayout.EndVertical();
            }
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
    }
}
