using DotEngine.Lua.UI;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.UI
{
    [CustomPropertyDrawer(typeof(EventHandlerData))]
    public class EventHanderDataPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect, label);

            Rect bindRect = new Rect(labelRect.x+20, labelRect.y+labelRect.height, labelRect.width-20, EditorGUIUtility.singleLineHeight);
            SerializedProperty bindBehaviourProperty = property.FindPropertyRelative("m_BindBehaviour");
            EditorGUI.PropertyField(bindRect, bindBehaviourProperty);

            Rect funcNameRect = bindRect;
            funcNameRect.y += bindRect.height;
            SerializedProperty funcNameProperty = property.FindPropertyRelative("m_FuncName");
            EditorGUI.PropertyField(funcNameRect, funcNameProperty);

        }
    }
}
