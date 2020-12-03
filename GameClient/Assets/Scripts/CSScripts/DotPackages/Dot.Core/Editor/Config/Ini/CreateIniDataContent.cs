using DotEditor.GUIExtension;
using DotEngine.Config.Ini;
using ReflectionMagic;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Config.Ini
{
    internal class CreateIniDataContent : PopupWindowContent
    {
        private IniGroup group = null;
        private Action<string, string> onCreatedCallback = null;
        private IniData newData = new IniData();

        private List<string> optionValues = new List<string>();
        private ReorderableList optionValuesRList = null;

        private string errorMessage = null;
        public CreateIniDataContent(IniGroup group,Action<string,string> callback) : base()
        {
            this.group = group;
            onCreatedCallback = callback;
        }

        public override void OnOpen()
        {
            base.OnOpen();

            optionValuesRList = new ReorderableList(optionValues, typeof(string), true, true, true, true);
            optionValuesRList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, Contents.OptionValuesHeadContent);
            };
            optionValuesRList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                string value = EditorGUI.TextField(rect, new GUIContent("" + index), optionValues[index]);
                if (value != optionValues[index])
                {
                    optionValues[index] = value;
                    newData.OptionValues = optionValues.ToArray();
                }
            };
            optionValuesRList.onAddCallback = (list) =>
            {
                optionValues.Add("");
                newData.OptionValues = optionValues.ToArray();
            };
        }

        private Vector2 scrollPos = Vector2.zero;
        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    EGUI.BeginLabelWidth(60);
                    {
                        EditorGUILayout.LabelField(Contents.NewDataContent, EGUIStyles.MiddleCenterLabel);

                        EditorGUILayout.Space();

                        string dataKey = newData.Key;
                        dataKey = EditorGUILayout.TextField(Contents.DataNameContent, dataKey);
                        if (newData.Key != dataKey)
                        {
                            newData.Key = dataKey;
                            if (string.IsNullOrEmpty(dataKey))
                            {
                                errorMessage = Contents.DataNameEmptyStr;
                            }
                            else
                            {

                                if ((group.AsDynamic()).dataDic.ContainsKey(newData.Key))
                                {
                                    errorMessage = Contents.DataNameRepeatStr;
                                }
                                else
                                {
                                    errorMessage = null;
                                }
                            }
                        }

                        if (newData.OptionValues != null && newData.OptionValues.Length > 0)
                        {
                            newData.Value = EGUILayout.StringPopup(Contents.DataValueContent, newData.Value, newData.OptionValues);
                        }
                        else
                        {
                            newData.Value = EditorGUILayout.TextField(Contents.DataValueContent, newData.Value);
                        }

                        newData.Comment = EditorGUILayout.TextField(Contents.DataCommentContent, newData.Comment);
                        optionValuesRList.DoLayoutList();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                        }
                        else
                        {
                            GUILayout.FlexibleSpace();

                            if (GUILayout.Button(Contents.SaveStr))
                            {
                                newData.OptionValues = optionValues.ToArray();

                                group.AddData(newData.Key, newData.Value, newData.Comment, newData.OptionValues);

                                onCreatedCallback?.Invoke(group.Name, newData.Key);
                                editorWindow.Close();
                            }
                        }
                    }
                    EGUI.EndLableWidth();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        static class Contents
        {
            internal static GUIContent NewDataContent = new GUIContent("Create New Data");
            internal static string DataNameEmptyStr = "The name of the data is Empty";
            internal static string DataNameRepeatStr = "The name of the data is exit in config";
            internal static GUIContent DataNameContent = new GUIContent("Name");
            internal static GUIContent DataCommentContent = new GUIContent("Comment");
            internal static GUIContent DataValueContent = new GUIContent("Value");

            internal static GUIContent OptionValuesHeadContent = new GUIContent("Option Values");

            internal static string SaveStr = "Save";
        }
    }
}
