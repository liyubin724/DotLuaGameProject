using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.GUIExtension.ColorfullFolders
{
    [CustomEditor(typeof(FavouriteFolderConfig))]
    public class FavouriteFolderConfigEditor : Editor
    {
        private ReorderableList colorFolderRList;
        private ReorderableList platformFolderRList;
        private ReorderableList tagFolderRList;
        private ReorderableList assetFolderRList;

        void OnEnable()
        {
            colorFolderRList = CreateRList("colorFolders");
            platformFolderRList = CreateRList("platformFolders");
            tagFolderRList = CreateRList("tagFolders");
            assetFolderRList = CreateRList("assetFolders");
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);
            
            serializedObject.Update();
            
            colorFolderRList.DoLayoutList();
            platformFolderRList.DoLayoutList();
            tagFolderRList.DoLayoutList();
            assetFolderRList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }


        private ReorderableList CreateRList(string propertyName)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            ReorderableList rList = new ReorderableList(serializedObject, property, true, true, true, true);
            rList.drawHeaderCallback = (rect) =>
              {
                  EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(propertyName), EGUIStyles.MiddleLeftLabelStyle);
              };
            rList.elementHeight = Styles.LIST_HEIGHT;
            rList.onAddCallback = (list) =>
            {
                property.InsertArrayElementAtIndex(property.arraySize);
            };
            rList.drawElementCallback = (rect, index, isActive, isFocused) =>
              {
                  SerializedProperty elementProperty = property.GetArrayElementAtIndex(index);

                  SerializedProperty enumProperty = elementProperty.FindPropertyRelative("Name");
                  SerializedProperty smallIconProperty = elementProperty.FindPropertyRelative("SmallIcon");
                  SerializedProperty largeIconProperty = elementProperty.FindPropertyRelative("LargeIcon");

                  EGUI.BeginLabelWidth(80);
                  {
                      Rect drawRect = rect;
                      drawRect.width -= Styles.LARGE_ICON_SIZE;
                      drawRect.height = Styles.LINE_HEIGHT;
                      EditorGUI.PropertyField(drawRect, enumProperty);

                      drawRect.y += drawRect.height;
                      EditorGUI.PropertyField(drawRect, smallIconProperty);

                      drawRect.y += drawRect.height;
                      EditorGUI.PropertyField(drawRect, largeIconProperty);
                  }
                  EGUI.EndLableWidth();

                  Rect previewRect = rect;
                  previewRect.x += previewRect.width - Styles.LARGE_ICON_SIZE;
                  previewRect.width = Styles.LARGE_ICON_SIZE;
                  previewRect.height = Styles.LARGE_ICON_SIZE;
                  UnityEngine.GUI.DrawTexture(previewRect, (Texture2D)largeIconProperty.objectReferenceValue ?? EGUIResources.DefaultFolderIcon);

                  previewRect.y += previewRect.height - Styles.SMALL_ICON_SIZE;
                  previewRect.width = previewRect.height = Styles.SMALL_ICON_SIZE;
                  UnityEngine.GUI.DrawTexture(previewRect, (Texture2D)smallIconProperty.objectReferenceValue ?? EGUIResources.DefaultFolderIcon);
              };

            return rList;
        }

        private static class Styles
        {
            internal static float LIST_HEIGHT = 64f;
            internal static float LINE_HEIGHT = 16f;
            internal static float SMALL_ICON_SIZE = 16f;
            internal static float LARGE_ICON_SIZE = 64f;
        }

    }
}
