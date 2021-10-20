using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;
namespace DotEditor.Asset.Dependency
{
    public class AssetDependencyWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Dependency Window", priority = 20)]
        public static void ShowWin()
        {
            AssetDependencyWindow win = EditorWindow.GetWindow<AssetDependencyWindow>();
            win.titleContent = new GUIContent("Bundle Packer");
            win.Show();
        }

        private AssetDependencyConfig assetDependencyConfig = null;
        private UnityObject selectedAsset = null;

        private GUIContent[] toolbarContents = new GUIContent[]
        {
            new GUIContent("Depends"),
            new GUIContent("UsedBy"),
        };
        private int toolbarSelectedIndex = 0;
        private bool isAutoRefresh = false;

        private AssetDependencyTreeView m_TreeView = null;

        private void OnEnable()
        {
            assetDependencyConfig = AssetDependencyUtil.GetAssetDependencyConfig();

            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.update += Repaint;
        }

        private void OnSelectionChanged()
        {
            if (Selection.activeObject != null
                && Selection.activeObject != selectedAsset
                && Selection.activeObject.GetType() != typeof(DefaultAsset)
                && EditorUtility.IsPersistent(Selection.activeObject))
            {
                if(Selection.activeObject!=selectedAsset)
                {
                    selectedAsset = Selection.activeObject;
                    if(isAutoRefresh)
                    {
                        RefreshTreeView();
                    }
                }
            }
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            EditorApplication.update -= Repaint;
        }

        private void OnGUI()
        {
            if (m_TreeView == null)
            {
                InitTreeView();
            }
            DrawToolbar();

            DrawSelectedAsset();
            DrawTreeViewToolbar();

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            m_TreeView.OnGUI(rect);
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal("toolbar", GUILayout.ExpandWidth(true));
            {
                if (GUILayout.Button("Reload", EditorStyles.toolbarButton, GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Warning", "This will take a lot of time.Are you sure?", "OK", "Cancel"))
                    {
                        assetDependencyConfig = AssetDependencyUtil.FindAllAssetData();
                        RefreshTreeView();
                    }
                }
                GUILayout.FlexibleSpace();

                isAutoRefresh = EditorGUILayout.Toggle("Auto Refresh", isAutoRefresh, EditorStyles.toggle);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void InitTreeView()
        {
            AssetDependencyTreeViewModel treeViewModel = new AssetDependencyTreeViewModel();
            treeViewModel.SetDependencyConfig(assetDependencyConfig);

            m_TreeView = new AssetDependencyTreeView(treeViewModel);
            RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            string assetPath = null;
            if (selectedAsset != null)
            {
                assetPath = AssetDatabase.GetAssetPath(selectedAsset);
            }

            if (string.IsNullOrEmpty(assetPath))
            {
                var viewMode = m_TreeView.GetViewModel<AssetDependencyTreeViewModel>();
                viewMode.Clear();
                m_TreeView.Reload();
            }
            else
            {
                var treeViewMode = m_TreeView.GetViewModel<AssetDependencyTreeViewModel>();
                if (toolbarSelectedIndex == 0)
                {
                    int[] expandIDs = treeViewMode.ShowAssets(new string[] { assetPath });
                    m_TreeView.Reload(expandIDs, null);
                }
                else if (toolbarSelectedIndex == 1)
                {
                    var usedDatas = assetDependencyConfig.GetBeUsedAssets(assetPath);

                    List<string> usedAssets = new List<string>();
                    if (usedDatas != null && usedDatas.Length > 0)
                    {
                        usedAssets.AddRange((from data in usedDatas select data.assetPath).ToArray());
                    }
                    treeViewMode.ShowAssets(usedAssets.ToArray());
                    m_TreeView.Reload();
                }
            }
        }

        private void DrawTreeViewToolbar()
        {
            int selectedIndex = GUILayout.Toolbar(toolbarSelectedIndex, toolbarContents, GUILayout.ExpandWidth(true), GUILayout.Height(40));
            if (selectedIndex != toolbarSelectedIndex)
            {
                toolbarSelectedIndex = selectedIndex;
                RefreshTreeView();
            }
        }

        private void DrawSelectedAsset()
        {
            EditorGUILayout.BeginHorizontal();
            {
                var newSelectedAsset = EditorGUILayout.ObjectField("Selected Asset", selectedAsset, typeof(UnityObject), false);
                if(newSelectedAsset != selectedAsset && isAutoRefresh)
                {
                    RefreshTreeView();
                }

                if(GUILayout.Button("Search ...",GUILayout.Width(80)))
                {
                    RefreshTreeView();
                }
            }
            EditorGUILayout.EndHorizontal();
            
        }
    }
}
