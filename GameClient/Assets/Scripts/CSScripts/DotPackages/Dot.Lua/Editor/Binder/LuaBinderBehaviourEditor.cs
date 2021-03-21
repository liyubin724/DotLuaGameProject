using DotEditor.GUIExt;
using DotEditor.GUIExt.IMGUI.RList;
using DotEditor.Utilities;
using DotEngine.Lua.Binder;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Binder
{
    [CustomEditor(typeof(LuaBinderBehaviour),false)]
    public class LuaBinderBehaviourEditor : Editor
    {
        SerializedProperty bindScriptProperty;
        SerializedProperty registParamsProperty;
        SerializedProperty registParamGroupsProperty;

        ReorderableListProperty registParamsRLProperty;
        List<ReorderableListProperty> registParamGroupsRLProperties = new List<ReorderableListProperty>();
        void OnEnable()
        {
            bindScriptProperty = serializedObject.FindProperty("binder");
            registParamsProperty = serializedObject.FindProperty("registParams");
            registParamGroupsProperty = serializedObject.FindProperty("registParamGroups");

            registParamsRLProperty = new ReorderableListProperty(registParamsProperty);
            RefreshRegistParamGroups();
        }

        private void RefreshRegistParamGroups()
        {
            registParamGroupsRLProperties.Clear();
            for (int i = 0; i < registParamGroupsProperty.arraySize; ++i)
            {
                SerializedProperty property = registParamGroupsProperty.GetArrayElementAtIndex(i);
                registParamGroupsRLProperties.Add(new ReorderableListProperty(property.FindPropertyRelative("paramList")));
            }
        }

        private int deleteIndex = -1;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.PropertyField(bindScriptProperty);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    registParamsRLProperty.OnGUILayout();
                }
                EditorGUILayout.EndVertical();

                if (deleteIndex >= 0)
                {
                    registParamGroupsProperty.RemoveElementAt(deleteIndex);
                    RefreshRegistParamGroups();
                    deleteIndex = -1;
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EGUILayout.DrawBoxHeader("Regist Param", GUILayout.ExpandWidth(true));
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    Rect addBtnRect = new Rect(lastRect.x + lastRect.width - 60, lastRect.y, 60, lastRect.height);
                    if (GUI.Button(addBtnRect, "Add"))
                    {
                        registParamGroupsProperty.InsertArrayElementAtIndex(registParamGroupsProperty.arraySize);
                        RefreshRegistParamGroups();
                    }

                    for (int i = 0; i < registParamGroupsProperty.arraySize; ++i)
                    {
                        SerializedProperty lopaProperty = registParamGroupsProperty.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                SerializedProperty nameProperty = lopaProperty.FindPropertyRelative("name");
                                EditorGUILayout.PropertyField(nameProperty);
                                registParamGroupsRLProperties[i].OnGUILayout();
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("Delete", GUILayout.Width(60)))
                            {
                                deleteIndex = i;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        EGUILayout.DrawHorizontalLine();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
