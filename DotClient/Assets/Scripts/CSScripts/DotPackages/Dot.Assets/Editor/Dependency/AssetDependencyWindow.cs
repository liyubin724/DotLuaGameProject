using DotEditor.GUIExtension;
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

        private AssetDependencyTreeView treeView = null;

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
            if (treeView == null)
            {
                InitTreeView();
            }
            DrawToolbar();

            EditorGUILayout.BeginHorizontal();
            {
                var newSelectedAsset = EditorGUILayout.ObjectField("Selected Asset", selectedAsset, typeof(UnityObject), false);
                if (newSelectedAsset != selectedAsset && isAutoRefresh)
                {
                    RefreshTreeView();
                }
                if(!isAutoRefresh)
                {
                    if (GUILayout.Button("Search ...", GUILayout.Width(80)))
                    {
                        RefreshTreeView();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            EGUI.DrawAreaLine(rect, Color.black);

            Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, 30);
            int selectedIndex = GUI.Toolbar(toolbarRect, toolbarSelectedIndex, toolbarContents);
            if (selectedIndex != toolbarSelectedIndex)
            {
                toolbarSelectedIndex = selectedIndex;
                RefreshTreeView();
            }

            Rect treeViewRect = new Rect(rect.x, rect.y + toolbarRect.height, rect.width, rect.height - toolbarRect.height);
            treeView.OnGUI(treeViewRect);
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

                var newAutoRefresh = EditorGUILayout.ToggleLeft("Auto Refresh", isAutoRefresh);
                if(newAutoRefresh!=isAutoRefresh)
                {
                    isAutoRefresh = newAutoRefresh;
                    RefreshTreeView();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void InitTreeView()
        {
            AssetDependencyTreeViewModel treeViewModel = new AssetDependencyTreeViewModel();
            treeViewModel.SetDependencyConfig(assetDependencyConfig);

            treeView = new AssetDependencyTreeView(treeViewModel);
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
                var viewMode = treeView.GetViewModel<AssetDependencyTreeViewModel>();
                viewMode.Clear();
                treeView.Reload();
            }
            else
            {
                var treeViewMode = treeView.GetViewModel<AssetDependencyTreeViewModel>();
                if (toolbarSelectedIndex == 0)
                {
                    int[] expandIDs = treeViewMode.ShowAssets(new string[] { assetPath });
                    treeView.Reload(expandIDs, null);
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
                    treeView.Reload();
                }
            }
        }
    }
}
