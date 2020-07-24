using DotEngine.Lua;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua
{
    [CustomPropertyDrawer(typeof(LuaBindScript))]
    public class LuaBindScriptPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty envNameProperty = property.FindPropertyRelative("m_EnvName");
            SerializedProperty scriptPathProperty = property.FindPropertyRelative("m_ScriptFilePath");

            Rect nameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(nameRect, envNameProperty);

            TextAsset scriptAsset = null;
            string scriptAssetPath = string.Empty;
            if (!string.IsNullOrEmpty(scriptPathProperty.stringValue))
            {
                scriptAssetPath = LuaConst.GetScriptAssetPath(scriptPathProperty.stringValue);
                scriptAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(scriptAssetPath);
                if (scriptAsset == null)
                {
                    scriptAsset = null;
                    scriptPathProperty.stringValue = null;
                    scriptAssetPath = string.Empty;
                }
            }

            Rect scriptRect = nameRect;
            scriptRect.y += nameRect.height;
            TextAsset newScriptAsset = (TextAsset)EditorGUI.ObjectField(scriptRect, "Lua Script", scriptAsset, typeof(TextAsset), false);

            scriptRect.y += scriptRect.height;
            scriptRect.x += 20;
            scriptRect.width -= 20;
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUI.TextField(scriptRect, "Script Path", scriptAssetPath);
            }
            EditorGUI.EndDisabledGroup();

            if (newScriptAsset != scriptAsset)
            {
                if (newScriptAsset == null)
                {
                    scriptPathProperty.stringValue = null;
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(newScriptAsset);
                    scriptPathProperty.stringValue = LuaConst.GetScriptPath(assetPath);
                }
            }
        }
    }
}