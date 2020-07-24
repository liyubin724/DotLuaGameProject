using DotEditor.GUIExtension;
using DotEngine.Config.Ini;
using ReflectionMagic;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Config.Ini
{
    internal class CreateIniGroupContent : PopupWindowContent
    {
        private IniConfig iniConfig = null;
        private IniGroup newGroup = new IniGroup();

        private string errorMessage = null;
        private Action<string> onCreatedCallback = null;
        public CreateIniGroupContent(IniConfig config, Action<string> callback):base()
        {
            iniConfig = config;
            onCreatedCallback = callback;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(300, 150);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.LabelField(Contents.NewGroupContent, EGUIStyles.MiddleCenterLabel);

                EditorGUILayout.Space();

                string groupName = newGroup.Name;
                groupName = EditorGUILayout.TextField(Contents.GroupNameContent, groupName);
                if (groupName != newGroup.Name)
                {
                    newGroup.Name = groupName;

                    if (string.IsNullOrEmpty(groupName))
                    {
                        errorMessage = Contents.GroupNameEmptyStr;
                    }
                    else
                    {
                        dynamic config = iniConfig.AsDynamic();
                        Dictionary<string, IniGroup> groupDic = config.groupDic;
                        if (groupDic.ContainsKey(newGroup.Name))
                        {
                            errorMessage = Contents.GroupNameRepeatStr;
                        }
                        else
                        {
                            errorMessage = null;
                        }
                    }
                }

                newGroup.Comment = EditorGUILayout.TextField(Contents.GroupCommentContent, newGroup.Comment);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                }
                else
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(Contents.SaveStr))
                    {
                        dynamic dynamicConfig = iniConfig.AsDynamic();
                        Dictionary<string, IniGroup> groupDic = dynamicConfig.groupDic;
                        groupDic.Add(newGroup.Name, newGroup);

                        onCreatedCallback?.Invoke(newGroup.Name);
                        editorWindow.Close();
                    }
                }
            }
            GUILayout.EndArea();
        }
        
        
        static class Contents
        {
            internal static GUIContent NewGroupContent = new GUIContent("Create New Group");
            internal static GUIContent GroupNameContent = new GUIContent("Name");
            internal static GUIContent GroupCommentContent = new GUIContent("Comment");
            internal static string GroupNameEmptyStr = "The name of the group is Empty";
            internal static string GroupNameRepeatStr = "The name of the group is exit in config";

            internal static string SaveStr = "Save";
        }
    }
}
