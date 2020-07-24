using DotEngine.UI;
using UnityEditor;

namespace DotEditor.UI
{
    [CustomEditor(typeof(UIAtlasImageAnimation), false)]
    public class UIAtlasImageAnimationEditor : UIAtlasImageEditor
    {
        private SerializedProperty isSetNativeSize;
        private SerializedProperty frameRate;
        private SerializedProperty autoPlayOnAwake;
        private SerializedProperty playMode;
        private SerializedProperty spriteNamePrefix;
        private SerializedProperty spriteIndex;
        private SerializedProperty spriteStartIndex;
        private SerializedProperty spriteEndIndex;
        protected override void OnEnable()
        {
            base.OnEnable();
            isSetNativeSize = serializedObject.FindProperty("isSetNativeSize");
            frameRate = serializedObject.FindProperty("frameRate");
            autoPlayOnAwake = serializedObject.FindProperty("autoPlayOnAwake");
            playMode = serializedObject.FindProperty("playMode");
            spriteNamePrefix = serializedObject.FindProperty("spriteNamePrefix");
            spriteIndex = serializedObject.FindProperty("spriteIndex");
            spriteStartIndex = serializedObject.FindProperty("spriteStartIndex");
            spriteEndIndex = serializedObject.FindProperty("spriteEndIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawSpriteAtlas();

            EditorGUILayout.PropertyField(frameRate);
            if (frameRate.intValue < 0)
            {
                frameRate.intValue = 0;
            }
            EditorGUILayout.PropertyField(autoPlayOnAwake);
            EditorGUILayout.PropertyField(playMode);
            EditorGUILayout.PropertyField(spriteNamePrefix);
            EditorGUILayout.PropertyField(spriteIndex);
            EditorGUILayout.PropertyField(spriteStartIndex);
            EditorGUILayout.PropertyField(spriteEndIndex);
            EditorGUILayout.PropertyField(isSetNativeSize);

            if (spriteIndex.intValue > spriteEndIndex.intValue || spriteIndex.intValue < spriteStartIndex.intValue)
            {
                spriteIndex.intValue = spriteStartIndex.intValue;
            }

            AppearanceControlsGUI();
            RaycastControlsGUI();
            DrawTypeGUI();
            DrawImageType();
            DrawNativeSize();

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnSpriteSelectedCallback(SerializedProperty spriteProperty, string spriteName)
        {
            spriteProperty.serializedObject.Update();
            if (string.IsNullOrEmpty(spriteName))
            {
                return;
            }

            int nDigitStartIndex = -1;
            for (int i = spriteName.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(spriteName[i]))
                {
                    nDigitStartIndex = i;
                    break;
                }
            }
            if (nDigitStartIndex > 0)
            {
                string prefix = spriteName.Substring(0, nDigitStartIndex + 1);
                if (prefix != spriteNamePrefix.stringValue)
                {
                    spriteNamePrefix.stringValue = prefix;
                }
                string index = spriteName.Substring(nDigitStartIndex + 1, spriteName.Length - nDigitStartIndex - 1);
                spriteIndex.intValue = int.Parse(index);
            }
            spriteProperty.stringValue = spriteName;
            spriteProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}
