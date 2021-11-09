using DotEngine.BMF;
using UnityEditor;
using UnityEngine;

namespace DotEditor.BMF
{
    [CustomEditor(typeof(BitmapFontTextMesh))]
    public class BitmapFontTextMeshEditor : BitmapFontTextEditor
    {
        SerializedProperty textMeshProperty;
        SerializedProperty meshRendererProperty;

        protected override void OnEnable()
        {
            textMeshProperty = serializedObject.FindProperty("textMesh");
            meshRendererProperty = serializedObject.FindProperty("meshRenderer");

            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            BitmapFontTextMesh bfTextMesh = (target as BitmapFontTextMesh);
            serializedObject.Update();
            {
                if (textMeshProperty.objectReferenceValue == null)
                {
                    textMeshProperty.objectReferenceValue = bfTextMesh.GetComponent<TextMesh>();
                }
                EditorGUILayout.PropertyField(textMeshProperty);
                if(meshRendererProperty.objectReferenceValue == null)
                {
                    meshRendererProperty.objectReferenceValue = bfTextMesh.GetComponent<MeshRenderer>();
                }
                EditorGUILayout.PropertyField(meshRendererProperty);
            }
            serializedObject.ApplyModifiedProperties();
            
            if(textMeshProperty.objectReferenceValue !=null)
            {
                base.OnInspectorGUI();
            }
        }
    }
}
