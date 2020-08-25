using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.Asset.Post.Rulers
{
    [CustomEditor(typeof(ExtractAndCompressClipFromFBXRuler))]
    public class ExtractAndCompressClipFromFBXRulerEditor : Editor
    {
        private SerializedProperty targetFolderProperty = null;
        private SerializedProperty precisionProperty = null;

        private void OnEnable()
        {
            targetFolderProperty = serializedObject.FindProperty("TargetFolder");
            precisionProperty = serializedObject.FindProperty("precision");
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);
            serializedObject.Update();
            {
                EGUILayout.DrawAssetFolderSelection(targetFolderProperty, true);

                EditorGUILayout.PropertyField(precisionProperty);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
