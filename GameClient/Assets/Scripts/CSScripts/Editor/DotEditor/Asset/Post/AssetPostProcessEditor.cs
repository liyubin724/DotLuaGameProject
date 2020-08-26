using DotEditor.GUIExtension;
using DotEditor.Utilities;
using DotEngine.Utilities;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DotEditor.Asset.Post
{
    [CustomEditor(typeof(AssetPostProcess))]
    public class AssetPostProcessEditor : Editor
    {
        private SerializedProperty typeProperty;
        private SerializedProperty filterProperty;
        private SerializedProperty rulerProperty;

        private ReorderableList rulerRList = null;
        private GenericMenu genericMenu = null;
        private void OnEnable()
        {
            typeProperty = serializedObject.FindProperty("Type");
            filterProperty = serializedObject.FindProperty("Filter");
            rulerProperty = serializedObject.FindProperty("Rulers");

            CreateMenu();
        }

        private void CreateMenu()
        {
            Type[] types = AssemblyUtility.GetDerivedTypes(typeof(AssetPostRuler));
            genericMenu = new GenericMenu();
            foreach(var type in types)
            {
                AssetPostRulerMenuAttribute attr = type.GetCustomAttribute<AssetPostRulerMenuAttribute>();
                if(attr!=null)
                {
                    genericMenu.AddItem(new GUIContent(attr.MenuName), false, (t) =>
                    {
                        var obj = ScriptableObject.CreateInstance((Type)t);
                        obj.name = attr.FileName;
                        AssetDatabase.AddObjectToAsset(obj, target);

                        rulerProperty.AddElement(obj);
                        
                        EditorUtility.SetDirty(target);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));
                    }, type);
                }
            }
            
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);

            serializedObject.Update();
            {
                EditorGUILayout.PropertyField(typeProperty);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(filterProperty);

                if(rulerRList == null)
                {
                    rulerRList = new ReorderableList(serializedObject, rulerProperty, true, true, true,true);
                    rulerRList.drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        SerializedProperty property = rulerProperty.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(rect, property);
                    };
                    rulerRList.drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, "Rulers");
                    };
                    rulerRList.onAddCallback = (list) =>
                    {
                        genericMenu.ShowAsContext();
                    };
                    rulerRList.onRemoveCallback = (list) =>
                    {
                        var removedObj = rulerProperty.RemoveElementAt(list.index);
                        if(removedObj!=null)
                        {
                            AssetDatabase.RemoveObjectFromAsset(removedObj);
                            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));

                            EditorUtility.SetDirty(target);
                        }
                    };
                }

                EditorGUILayout.Space();

                rulerRList.DoLayoutList();
            }
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Space();

            if (GUILayout.Button("Execute",GUILayout.Height(40)))
            {
                (target as AssetPostProcess).Process();
            }
        }
    }
}
