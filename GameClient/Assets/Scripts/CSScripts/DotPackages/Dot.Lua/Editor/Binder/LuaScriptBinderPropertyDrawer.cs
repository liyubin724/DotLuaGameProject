using DotEngine.Lua;
using DotEngine.Lua.Binder;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Binder
{
    [CustomPropertyDrawer(typeof(LuaScriptBinder))]
    public class LuaScriptBinderPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty scriptPathProperty = property.FindPropertyRelative("scriptPath");

            TextAsset scriptAsset = null;
            string scriptAssetPath = string.Empty;
            if (!string.IsNullOrEmpty(scriptPathProperty.stringValue))
            {
                scriptAssetPath = LuaUtility.GetScriptAssetPath(scriptPathProperty.stringValue);
                scriptAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(scriptAssetPath);
                if (scriptAsset == null)
                {
                    scriptAsset = null;
                    scriptPathProperty.stringValue = null;
                    scriptAssetPath = string.Empty;
                }
            }

            Rect scriptRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
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
                    scriptPathProperty.stringValue = LuaUtility.GetScriptPath(assetPath);
                }
            }
        }
    }
}