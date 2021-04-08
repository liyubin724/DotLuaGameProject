﻿using DotEditor.GUIExt.IMGUI.RList;
using DotEngine.Lua.UI;
using UnityEditor;
using UnityEditor.UI;

namespace DotEditor.Lua.UI
{
    [CustomEditor(typeof(LuaButton), true)]
    public class LuaButtonEditor : ButtonEditor
    {
        SerializedProperty binderBehaviourProperty;
        SerializedProperty clickedFuncNameProperty;
        SerializedProperty paramValuesProperty;

        ReorderableListProperty paramValuesRLProperty;
        protected override void OnEnable()
        {
            base.OnEnable();
            binderBehaviourProperty = serializedObject.FindProperty("binderBehaviour");
            clickedFuncNameProperty = serializedObject.FindProperty("clickedFuncName");
            paramValuesProperty = serializedObject.FindProperty("paramValues");

            paramValuesRLProperty = new ReorderableListProperty(paramValuesProperty);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(binderBehaviourProperty);
                EditorGUILayout.PropertyField(clickedFuncNameProperty);

                paramValuesRLProperty.OnGUILayout();
            }
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}