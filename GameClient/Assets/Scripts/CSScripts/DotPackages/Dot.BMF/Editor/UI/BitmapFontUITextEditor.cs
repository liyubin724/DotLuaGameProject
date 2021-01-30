using DotEditor.BMF;
using DotEngine.BMF.UI;
using UnityEditor;
using UnityEngine.UI;

namespace DotEditor.BMF.UI
{
    [CustomEditor(typeof(BitmapFontUIText))]
    public class BitmapFontUITextEditor : BitmapFontTextEditor
    {
        SerializedProperty uiTextProperty;

        protected override void OnEnable()
        {
            uiTextProperty = serializedObject.FindProperty("uiText");

            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                if (uiTextProperty.objectReferenceValue == null)
                {
                    uiTextProperty.objectReferenceValue = (target as BitmapFontUIText).GetComponent<Text>();
                }
                EditorGUILayout.PropertyField(uiTextProperty);
            }
            serializedObject.ApplyModifiedProperties();

            if (uiTextProperty.objectReferenceValue != null)
            {
                base.OnInspectorGUI();
            }
        }
    }
}