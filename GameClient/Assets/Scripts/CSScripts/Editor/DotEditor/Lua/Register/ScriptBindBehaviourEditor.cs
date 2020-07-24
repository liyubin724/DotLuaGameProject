using DotEngine.Lua.Register;
using UnityEditor;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ScriptBindBehaviour))]
    public class ScriptBindBehaviourEditor : Editor
    {
        SerializedProperty bindScriptProperty;

        protected virtual void OnEnable()
        {
            bindScriptProperty = serializedObject.FindProperty("bindScript");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(bindScriptProperty);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
