using DotEngine.Lua.Binder;
using UnityEditor;

namespace DotEditor.Lua.Binder
{
    [CustomEditor(typeof(LuaBinderBehaviour),false)]
    public class LuaBinderBehaviourEditor : Editor
    {
        SerializedProperty bindScriptProperty;
        protected virtual void OnEnable()
        {
            bindScriptProperty = serializedObject.FindProperty("binder");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.PropertyField(bindScriptProperty);
                }
                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
