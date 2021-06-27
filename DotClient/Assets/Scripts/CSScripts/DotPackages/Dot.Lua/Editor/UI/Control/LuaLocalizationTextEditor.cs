using DotEngine.Lua.UI;
using UnityEditor;

namespace DotEditor.Lua.UI
{
    [CustomEditor(typeof(LuaLocalizationText))]
    public class LuaLocalizationTextEditor : UnityEditor.UI.TextEditor
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
