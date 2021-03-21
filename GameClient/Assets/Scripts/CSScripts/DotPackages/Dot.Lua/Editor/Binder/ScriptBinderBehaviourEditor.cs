using DotEditor.GUIExtension.RList;
using DotEngine.Lua.Binder;
using UnityEditor;

namespace DotEditor.Lua.Binder
{
    [CustomEditor(typeof(ScriptBinderBehaviour),true)]
    public class ScriptBinderBehaviourEditor : Editor
    {
        SerializedProperty bindScriptProperty;
        SerializedProperty constructorParamsProperty;

        ReorderableListProperty constructorParamsRLProperty;
        protected virtual void OnEnable()
        {
            bindScriptProperty = serializedObject.FindProperty("bindScript");
            constructorParamsProperty = serializedObject.FindProperty("constructorParams");
            constructorParamsRLProperty = new ReorderableListProperty(constructorParamsProperty);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.PropertyField(bindScriptProperty);
                    EditorGUILayout.Space();
                    constructorParamsRLProperty.OnGUILayout();
                }
                EditorGUILayout.EndVertical();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
