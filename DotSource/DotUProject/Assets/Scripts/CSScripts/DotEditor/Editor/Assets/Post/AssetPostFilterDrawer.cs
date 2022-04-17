using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;
using static DotEditor.Assets.Post.AssetPostProcess;

namespace DotEditor.Assets.Post
{
    [CustomPropertyDrawer(typeof(AssetPostFilter))]
    public class AssetPostFilterDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect drawRect = position;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(drawRect, label,EGUIStyles.BoxedHeaderStyle);

            SerializedProperty folderProperty = property.FindPropertyRelative("Folder");
            SerializedProperty IsIncludeSubProperty = property.FindPropertyRelative("IsIncludeSub");
            SerializedProperty fileNameRegexProperty = property.FindPropertyRelative("FileNameRegex");

            drawRect.x += 20;
            drawRect.width -= 20;
            drawRect.y += drawRect.height;
            EGUI.DrawAssetFolderSelection(drawRect, folderProperty, true);

            drawRect.y += drawRect.height;
            EditorGUI.PropertyField(drawRect,IsIncludeSubProperty);

            drawRect.y += drawRect.height;
            EditorGUI.PropertyField(drawRect, fileNameRegexProperty);
        }
    }
}
