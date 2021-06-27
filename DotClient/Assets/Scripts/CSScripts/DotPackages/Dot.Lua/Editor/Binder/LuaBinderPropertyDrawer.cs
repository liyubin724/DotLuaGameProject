using DotEditor.GUIExt;
using DotEditor.GUIExt.IMGUI.RList;
using DotEngine.Lua;
using DotEngine.Lua.Binder;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Binder
{
    [CustomPropertyDrawer(typeof(LuaBinder))]
    public class LuaBinderPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, ReorderableListProperty> rListDic = new Dictionary<string, ReorderableListProperty>();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * 7;

            SerializedProperty paramValuesProperty = property.FindPropertyRelative("paramValues");
            for(int i =0;i<paramValuesProperty.arraySize;++i)
            {
                SerializedProperty childProperty = paramValuesProperty.GetArrayElementAtIndex(i);
                height  += EditorGUIUtility.singleLineHeight * 4;

                SerializedProperty paramTypeProperty = childProperty.FindPropertyRelative("paramType");
                if (paramTypeProperty.intValue == (int)LuaParamType.UObject)
                {
                    SerializedProperty gObjectProperty = childProperty.FindPropertyRelative("gObject");
                    if (gObjectProperty.objectReferenceValue != null)
                    {
                        height += EditorGUIUtility.singleLineHeight * 2;
                    }
                }
            }
            return height;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty scriptPathProperty = property.FindPropertyRelative("scriptPath");
            SerializedProperty paramValuesProperty = property.FindPropertyRelative("paramValues");

            Rect headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EGUI.DrawBoxHeader(headerRect, label.text);

            Rect scriptRect = headerRect;
            scriptRect.y += headerRect.height;
            scriptRect.height = EditorGUIUtility.singleLineHeight * 2;
            DrawScriptProperty(scriptRect, scriptPathProperty);

            Rect paramValuesRect = scriptRect;
            paramValuesRect.y += scriptRect.height;
            paramValuesRect.height = position.height - headerRect.height - scriptRect.height;

            string paramValuesPropertyPath = paramValuesProperty.propertyPath;
            if(!rListDic.TryGetValue(paramValuesPropertyPath,out var rList))
            {
                rList = new ReorderableListProperty(paramValuesProperty);
                rListDic.Add(paramValuesPropertyPath, rList);
            }
            rList.OnGUI(paramValuesRect);
        }

        private void DrawScriptProperty(Rect position, SerializedProperty scriptPathProperty)
        {
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
            TextAsset newScriptAsset = (TextAsset)EditorGUI.ObjectField(scriptRect, "Lua Asset", scriptAsset, typeof(TextAsset), false);

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