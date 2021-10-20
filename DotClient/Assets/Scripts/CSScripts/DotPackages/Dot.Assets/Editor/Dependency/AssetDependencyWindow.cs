using DotEditor.GUIExtension;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;
namespace DotEditor.Asset.Dependency
{
    public class IngoreAssetExtension
    {
        public string displayName = "";
        public string extension = "";
        public bool isSelected = false;
    }

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
        private IngoreAssetExtension[] ingoreAssetExtensions = new IngoreAssetExtension[]
        {
            new IngoreAssetExtension()
            {
                displayName = "txt",
                extension = ".txt",
                isSelected = true,
            },
            new IngoreAssetExtension()
            {
                displayName = "cs",
                extension = ".cs,.dll,.m,.cpp,.c,.h,.lua",
                isSelected = true,
            },
            new IngoreAssetExtension()
            {
                displayName = "Shader",
                extension = ".shader,.cginc",
                isSelected = false,
            },
        };

        private GUIContent[] toolbarContents = new GUIContent[]
        {
            new GUIContent("DependOn"),
            new GUIContent("UsedBy"),
        };
        private int toolbarSelectedIndex = 0;

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
                selectedAsset = Selection.activeObject;
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
            
            DrawDependency();
            DrawSelectedAsset();
            DrawIngoreAssetExtension();
            DrawTreeViewToolbar();

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            m_TreeView.OnGUI(rect);
        }

        private void InitTreeView()
        {
            AssetDependencyTreeViewModel treeViewModel = new AssetDependencyTreeViewModel();
            treeViewModel.SetDependencyConfig(assetDependencyConfig);
            treeViewModel.SetIgnoreExtension(GetSelectedIgnoreExtensions());

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
                int[] expandIDs = m_TreeView.GetViewModel<AssetDependencyTreeViewModel>().ShowDependency(new string[0]);
                m_TreeView.Reload(expandIDs, null);
            }
            else
            {
                if (toolbarSelectedIndex == 0)
                {
                    int[] expandIDs = m_TreeView.GetViewModel<AssetDependencyTreeViewModel>().ShowDependency(new string[] { assetPath });
                    m_TreeView.Reload(expandIDs, new int[0]);
                }
                else if (toolbarSelectedIndex == 1)
                {
                    var usedDatas = assetDependencyConfig.GetBeUsedAssets(assetPath);

                    List<string> usedAssets = new List<string>();
                    if (usedDatas != null && usedDatas.Length > 0)
                    {
                        usedAssets.AddRange((from data in usedDatas select data.assetPath).ToArray());
                    }
                    m_TreeView.GetViewModel<AssetDependencyTreeViewModel>().ShowDependency(usedAssets.ToArray());
                    m_TreeView.GetViewModel<AssetDependencyTreeViewModel>().ShowSelected(assetPath,out var expandIDs,out var selectedIDs);

                    m_TreeView.Reload(expandIDs.ToArray(), selectedIDs.ToArray());
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

        private void DrawDependency()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Reload", GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Warning", "This will take a lot of time.Are you sure?", "OK", "Cancel"))
                    {
                        assetDependencyConfig = AssetDependencyUtil.FindAllAssetData();
                        RefreshTreeView();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSelectedAsset()
        {
            EditorGUILayout.BeginHorizontal();
            {
                selectedAsset = EditorGUILayout.ObjectField("Selected Asset", selectedAsset, typeof(UnityObject), false);

                if(GUILayout.Button("Search ...",GUILayout.Width(80)))
                {
                    RefreshTreeView();
                }
                EditorGUI.BeginDisabledGroup(toolbarSelectedIndex != 1);
                {
                    if (GUILayout.Button("Selected ...", GUILayout.Width(80)))
                    {
                        RefreshTreeView();
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
            
        }

        private void DrawIngoreAssetExtension()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Ignore Asset Extension:");
                EGUI.BeginIndent();
                {
                    EditorGUI.BeginChangeCheck();
                    {
                        foreach (var extension in ingoreAssetExtensions)
                        {
                            extension.isSelected = EditorGUILayout.Toggle(extension.displayName, extension.isSelected);
                        }
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_TreeView.GetViewModel<AssetDependencyTreeViewModel>().SetIgnoreExtension(GetSelectedIgnoreExtensions());
                    }
                }
                EGUI.EndIndent();
            }
            EditorGUILayout.EndVertical();
        }

        private string[] GetSelectedIgnoreExtensions()
        {
            return (from extension in ingoreAssetExtensions
                    where extension.isSelected
                    let exts = extension.extension.Split(new char[] { ',' })
                    from ext in exts
                    select ext
                    ).ToArray();
        }
    }

    class Contents
    {

    }
}
