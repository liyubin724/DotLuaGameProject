using DotEditor.GUIExt.IMGUI.RList;
using DotEngine.Lua.UI;
using UnityEditor;
using UnityEditor.UI;

namespace DotEditor.Lua.UI
{
    [CustomEditor(typeof(LuaInputField),true)]
    public class LuaInputFieldEditor : InputFieldEditor
    {
        SerializedProperty binderBehaviourProperty;

        SerializedProperty changedFuncNameProperty;
        SerializedProperty changedParamValuesProperty;

        SerializedProperty submitedFuncNameProperty;
        SerializedProperty submitedParamValuesProperty;

        ReorderableListProperty changedParamValuesRLProperty;
        ReorderableListProperty submitedParamValuesRLProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            binderBehaviourProperty = serializedObject.FindProperty("binderBehaviour");

            changedFuncNameProperty = serializedObject.FindProperty("changedFuncName");
            changedParamValuesProperty = serializedObject.FindProperty("changedParamValues");

            submitedFuncNameProperty = serializedObject.FindProperty("submitedFuncName");
            submitedParamValuesProperty = serializedObject.FindProperty("submitedParamValues");

            changedParamValuesRLProperty = new ReorderableListProperty(changedParamValuesProperty);
            submitedParamValuesRLProperty = new ReorderableListProperty(submitedParamValuesProperty);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(binderBehaviourProperty);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(changedFuncNameProperty);
                changedParamValuesRLProperty.OnGUILayout();
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(submitedFuncNameProperty);
                submitedParamValuesRLProperty.OnGUILayout();
            }
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
