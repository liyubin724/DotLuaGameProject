using DotEditor.GUIExtension;
using DotEditor.GUIExtension.RList;
using DotEditor.Utilities;
using DotEngine.Lua.Binder;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Binder
{
    [CustomEditor(typeof(ObjectBinderBehaviour), true)]
    public class ObjectBinderBehaviourEditor : ScriptBinderBehaviourEditor
    {
        SerializedProperty registObjectsProperty = null;
        SerializedProperty registArrayObjectsProperty = null;

        ReorderableListProperty registObjectsRLProperty = null;
        List<ReorderableListProperty> registArrayObjectsRLProperties = new List<ReorderableListProperty>();
        protected override void OnEnable()
        {
            base.OnEnable();
            registObjectsProperty = serializedObject.FindProperty("m_RegistObjects");
            registObjectsRLProperty = new ReorderableListProperty(registObjectsProperty);

            registArrayObjectsProperty = serializedObject.FindProperty("m_RegistArrayObjects");
            CreateRegistArrayObjectsRLProperty();
        }

        private void CreateRegistArrayObjectsRLProperty()
        {
            registArrayObjectsRLProperties.Clear();
            for (int i = 0; i < registArrayObjectsProperty.arraySize; ++i)
            {
                SerializedProperty property = registArrayObjectsProperty.GetArrayElementAtIndex(i);
                registArrayObjectsRLProperties.Add(new ReorderableListProperty(property.FindPropertyRelative("operateParams")));
            }
        }

        private int m_DeleteArrayObjectIndex = -1;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            {
                if (m_DeleteArrayObjectIndex >= 0)
                {
                    registArrayObjectsProperty.RemoveElementAt(m_DeleteArrayObjectIndex);
                    CreateRegistArrayObjectsRLProperty();
                    m_DeleteArrayObjectIndex = -1;
                }

                registObjectsRLProperty.OnGUILayout();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EGUILayout.DrawBoxHeader("Regist Array Objects", GUILayout.ExpandWidth(true));
                    Rect lastRect = GUILayoutUtility.GetLastRect();
                    Rect addBtnRect = new Rect(lastRect.x + lastRect.width - 60, lastRect.y, 60, lastRect.height);
                    if (GUI.Button(addBtnRect, "Add"))
                    {
                        registArrayObjectsProperty.InsertArrayElementAtIndex(registArrayObjectsProperty.arraySize);
                        CreateRegistArrayObjectsRLProperty();
                    }

                    for (int i = 0; i < registArrayObjectsProperty.arraySize; ++i)
                    {
                        SerializedProperty lopaProperty = registArrayObjectsProperty.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                SerializedProperty nameProperty = lopaProperty.FindPropertyRelative("name");
                                EditorGUILayout.PropertyField(nameProperty);
                                registArrayObjectsRLProperties[i].OnGUILayout();
                            }
                            EditorGUILayout.EndVertical();

                            if (GUILayout.Button("Delete", GUILayout.Width(60)))
                            {
                                m_DeleteArrayObjectIndex = i;
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
