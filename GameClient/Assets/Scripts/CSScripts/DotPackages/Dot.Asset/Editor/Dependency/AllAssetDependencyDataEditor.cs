using DotEditor.GUIExtension;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Asset.Dependency
{
    [CustomEditor(typeof(AllAssetDependencyData))]
    public class AllAssetDependencyDataEditor : Editor
    {
        private SerializedProperty assetDatasProperty = null;
        private SearchField searchField = null;
        private string searchText = string.Empty;

        private List<SerializedProperty> searchResults = new List<SerializedProperty>();
        private int searchToolbarIndex = 0;
        private void OnEnable()
        {
            assetDatasProperty = serializedObject.FindProperty("assetDatas");
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);
            EditorGUILayout.Space();

            //if(searchField == null)
            //{
            //    searchField = new SearchField();
            //}
            //Rect searchRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.Height(40));
            //string text = searchField.OnGUI(searchRect,searchText);
            //if(text!= searchText)
            //{
            //    searchText = text;
            //    searchResults.Clear();
            //    if(!string.IsNullOrEmpty(searchText))
            //    {
            //        for(int i =0;i<assetDatasProperty.arraySize;++i)
            //        {
            //            SerializedProperty dataProperty = assetDatasProperty.GetArrayElementAtIndex(i);
            //            string assetPath = dataProperty.FindPropertyRelative("assetPath").stringValue;
            //            if(assetPath.ToLower().IndexOf(searchText.ToLower()) >= 0)
            //            {
            //                searchResults.Add(dataProperty.Copy());
            //            }
            //        }
            //    }
            //}
            //foreach(var data in searchResults)
            //{
            //    EditorGUILayout.PropertyField(data,true);
            //}
        }
    }
}
