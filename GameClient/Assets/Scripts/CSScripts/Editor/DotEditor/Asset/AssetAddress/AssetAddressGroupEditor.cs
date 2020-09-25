using DotEditor.GUIExtension;
using DotEditor.GUIExtension.TreeGUI;
using DotEditor.NativeDrawer;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.AssetAddress
{
    [CustomEditor(typeof(AssetAddressGroup))]
    public class AssetAddressGroupEditor :  NativeDrawerEditor
    {
        private SimpleListView<string> listViewer = null;

        protected override bool IsShowScroll()
        {
            return false;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                if (GUILayout.Button("Execute", GUILayout.Height(40)))
                {
                    AssetAddressUtil.UpdateConfigByGroup(target as AssetAddressGroup);
                    EditorUtility.DisplayDialog("Finished", "Finished", "OK");
                }

                if (GUILayout.Button("Filter", GUILayout.Height(40)))
                {
                    List<string> files = new List<string>();
                    AssetAddressGroup group = target as AssetAddressGroup;
                    foreach (var filter in group.filters)
                    {
                        files.AddRange(filter.Filter());
                    }

                    listViewer = new SimpleListView<string>();
                    listViewer.Header = "Asset List";
                    listViewer.OnDrawItem = (rect, index) =>
                    {
                        Rect indexRect = new Rect(rect.x, rect.y, 20, rect.height);
                        EditorGUI.PrefixLabel(indexRect, new GUIContent("" + index));
                        Rect itemRect = new Rect(rect.x + indexRect.width, rect.y, rect.width - indexRect.width, rect.height);
                        EditorGUI.LabelField(itemRect, listViewer.GetItem(index));
                    };
                    listViewer.AddItems(files.ToArray());
                }

                EGUILayout.DrawHorizontalLine();

                if (listViewer != null)
                {
                    Rect lastRect = EditorGUILayout.GetControlRect(false,300,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    if (Event.current.type == EventType.Repaint)
                    {
                        listViewer.OnGUI(lastRect);
                    }
                }
            }
            EditorGUILayout.EndVertical();
            
        }
    }
}
