using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Asset.Post
{
    [CustomEditor(typeof(AssetPostProcess))]
    public class AssetPostProcessEditor : Editor
    {
        private SerializedProperty filterProperty;
        private SerializedProperty rulerProperty;

        private ReorderableList rulerRList = null;
        private void OnEnable()
        {
            filterProperty = serializedObject.FindProperty("Filter");
            rulerProperty = serializedObject.FindProperty("Rulers");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(filterProperty);

                if(rulerRList == null)
                {
                    rulerRList = new ReorderableList(serializedObject, rulerProperty, true, true, true,true);
                    rulerRList.drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        SerializedProperty property = rulerProperty.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(rect, property);
                    };
                    rulerRList.drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, "Rulers");
                    };
                    rulerRList.onAddCallback = (list) =>
                    {
                        rulerProperty.InsertArrayElementAtIndex(rulerProperty.arraySize);
                    };
                }

                rulerRList.DoLayoutList();
            }
            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Execute",GUILayout.Height(40)))
            {
                (target as AssetPostProcess).Process();
            }
        }
    }
}
