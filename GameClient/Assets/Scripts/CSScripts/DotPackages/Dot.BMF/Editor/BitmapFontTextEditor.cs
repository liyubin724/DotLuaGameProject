using DotEditor.Utilities;
using DotEngine.BMF;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Fonts
{
    public class BitmapFontTextEditor : Editor
    {
        SerializedProperty fontDataProperty;
        SerializedProperty fontNameProperty;
        SerializedProperty textProperty;

        private BitmapFont[] fontDatas = null;
        private string[] fontDataNames = null;
        protected virtual void OnEnable()
        {
            fontDataProperty = serializedObject.FindProperty("m_FontData");
            fontNameProperty = serializedObject.FindProperty("m_FontName");
            textProperty = serializedObject.FindProperty("m_Text");

            string[] fontPaths = AssetDatabaseUtility.FindAssets<BitmapFont>();
            fontDataNames = new string[fontPaths.Length + 1];
            fontDatas = new BitmapFont[fontPaths.Length + 1];
            
            fontDataNames[0] = "--None--";
            fontDatas[0] = null;

            for(int i =0;i<fontPaths.Length;++i)
            {
                fontDataNames[i + 1] = Path.GetFileNameWithoutExtension(fontPaths[i]);
                fontDatas[i + 1] = AssetDatabase.LoadAssetAtPath<BitmapFont>(fontPaths[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            {
                OnDrawFont();
                OnDrawFontNames();

                EditorGUILayout.PropertyField(textProperty);
            }
            serializedObject.ApplyModifiedProperties();

            if(GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void OnDrawFont()
        {
            BitmapFont fontData = (BitmapFont)fontDataProperty.objectReferenceValue;
            int selectedIndex = Array.IndexOf(fontDatas, fontData);
            int newSelectedIndex = EditorGUILayout.Popup("Font Data", selectedIndex, fontDataNames);
            if (newSelectedIndex != selectedIndex)
            {
                selectedIndex = newSelectedIndex;
                fontDataProperty.objectReferenceValue = fontDatas[selectedIndex];
            }
            if (fontData != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUILayout.PropertyField(fontDataProperty);
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        private void OnDrawFontNames()
        {
            BitmapFont fontData = (BitmapFont)fontDataProperty.objectReferenceValue;
            if(fontData!=null)
            {
                string[] names = (from charMap in fontData.charMaps select charMap.name).ToArray();
                int selectedIndex = Array.IndexOf(names, fontNameProperty.stringValue);
                
                int newSelectedIndex = EditorGUILayout.Popup("Font Name", selectedIndex,names);
                if(newSelectedIndex!=selectedIndex)
                {
                    selectedIndex = newSelectedIndex;
                    fontNameProperty.stringValue = names[selectedIndex];
                }
            }
        }
    }
}
