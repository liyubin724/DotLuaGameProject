using DotEngine.Lua.UI;
using TMPro.EditorUtilities;
using UnityEditor;

namespace DotEditor.Lua.UI
{
    [CustomEditor(typeof(LuaLocalizationTMText))]
    public class LuaLocalizationTMTextEditor: TMP_EditorPanelUI
    {
        SerializedProperty localizationNameProperty = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            localizationNameProperty = serializedObject.FindProperty("localizationName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(localizationNameProperty);
                EditorGUILayout.Space();
            }
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
