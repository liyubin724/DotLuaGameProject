using DotEditor.GUIExtension;
using DotEditor.GUIExtension.RList;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestRList))]
public class TestRListEditor : Editor
{
    private SerializedProperty intValuesProperty;
    private ReorderableListProperty rlProperty = null;

    private void OnEnable()
    {
        intValuesProperty = serializedObject.FindProperty("intValues");
        rlProperty = new ReorderableListProperty(intValuesProperty);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        {
            rlProperty.OnGUILayout();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
