using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.Asset.Post.Rulers
{
    [CustomEditor(typeof(ExtractClipFromFBXRuler))]
    public class ExtractClipFromFBXRulerEditor : Editor
    {
        private SerializedProperty targetFolderProperty = null;

        private void OnEnable()
        {
            targetFolderProperty = serializedObject.FindProperty("TargetFolder");
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);
            serializedObject.Update();
            {
                EGUILayout.DrawAssetFolderSelection(targetFolderProperty, true);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
