using DotEditor.GUIExtension;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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

        private AllAssetDependencyData m_AllAssetData = null;
        private UnityObject m_SelectedAsset = null;
        private IngoreAssetExtension[] m_IngoreAssetExtensions = new IngoreAssetExtension[]
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
                extension = ".cs,.dll",
                isSelected = true,
            },
            new IngoreAssetExtension()
            {
                displayName = "lua",
                extension = ".txt,.lua",
                isSelected = true,
            },
            new IngoreAssetExtension()
            {
                displayName = "Shader",
                extension = ".shader,.cginc",
                isSelected = false,
            },
        };

        private GUIContent[] m_ToolbarContents = new GUIContent[]
        {
            new GUIContent("DependOn"),
            new GUIContent("UsedBy"),
        };
        private int m_ToolbarSelectedIndex = 0;

        private AssetDependencyTreeView m_TreeView = null;

        private void OnEnable()
        {
            m_AllAssetData = AssetDependencyUtil.GetOrCreateAllAssetData();
            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.update += Repaint;
        }

        private void OnSelectionChanged()
        {
            if (Selection.activeObject != null
                && Selection.activeObject != m_SelectedAsset
                && Selection.activeObject.GetType() != typeof(DefaultAsset)
                && EditorUtility.IsPersistent(Selection.activeObject))
            {
                m_SelectedAsset = Selection.activeObject;
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
            
            DrawAllAssetDependency();
            DrawSelectedAsset();
            DrawIngoreAssetExtension();
            DrawTreeViewToolbar();

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            m_TreeView.OnGUI(rect);
        }

        private void InitTreeView()
        {
            TreeViewState treeViewState = new TreeViewState();
            AssetDependencyTreeViewModel treeViewModel = new AssetDependencyTreeViewModel();
            treeViewModel.SetIgnoreExtension(GetSelectedIgnoreExtensions());

            m_TreeView = new AssetDependencyTreeView(treeViewState,treeViewModel);
            RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            string assetPath = null;
            if (m_SelectedAsset != null)
            {
                assetPath = AssetDatabase.GetAssetPath(m_SelectedAsset);
            }

            if (string.IsNullOrEmpty(assetPath))
            {
                int[] expandIDs = m_TreeView.GetModel<AssetDependencyTreeViewModel>().ShowDependency(new string[0]);
                m_TreeView.Reload(expandIDs, null);
            }
            else
            {
                if (m_ToolbarSelectedIndex == 0)
                {
                    int[] expandIDs = m_TreeView.GetModel<AssetDependencyTreeViewModel>().ShowDependency(new string[] { assetPath });
                    m_TreeView.Reload(expandIDs, null);
                }
                else if (m_ToolbarSelectedIndex == 1)
                {
                    var usedDatas = AssetDependencyUtil.GetAssetUsedBy(assetPath, (title, message, progress) =>
                    {
                        EditorUtility.DisplayProgressBar(title, message, progress);
                    });
                    EditorUtility.ClearProgressBar();

                    List<string> usedAssets = new List<string>();
                    if (usedDatas != null && usedDatas.Length > 0)
                    {
                        usedAssets.AddRange((from data in usedDatas select data.assetPath).ToArray());
                    }
                    m_TreeView.GetModel<AssetDependencyTreeViewModel>().ShowDependency(usedAssets.ToArray());
                    m_TreeView.GetModel<AssetDependencyTreeViewModel>().ShowSelected(assetPath,out var expandIDs,out var selectedIDs);

                    m_TreeView.Reload(expandIDs.ToArray(), selectedIDs.ToArray());
                }
            }
        }

        private void DrawTreeViewToolbar()
        {
            int selectedIndex = GUILayout.Toolbar(m_ToolbarSelectedIndex, m_ToolbarContents, GUILayout.ExpandWidth(true), GUILayout.Height(40));
            if (selectedIndex != m_ToolbarSelectedIndex)
            {
                m_ToolbarSelectedIndex = selectedIndex;
                RefreshTreeView();
            }
        }

        private void DrawAllAssetDependency()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.ObjectField("All Asset Data", m_AllAssetData, typeof(AllAssetDependencyData), false);
                if (GUILayout.Button("Reload", GUILayout.Width(80)))
                {
                    if (EditorUtility.DisplayDialog("Warning", "This will take a lot of time.Are you sure?", "OK", "Cancel"))
                    {
                        AssetDependencyUtil.FindAllAssetData((title, message, progress) =>
                        {
                            EditorUtility.DisplayProgressBar(title, message, progress);
                        });
                        EditorUtility.ClearProgressBar();

                        if (!EditorUtility.IsPersistent(m_AllAssetData) && EditorUtility.DisplayDialog("Save As", "Do you want to save it?", "OK"))
                        {
                            string filePath = EditorUtility.SaveFilePanelInProject("save", "all_asset_dependency", "asset", "Save data as a asset");
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                AssetDatabase.CreateAsset(m_AllAssetData, filePath);
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSelectedAsset()
        {
            EditorGUILayout.BeginHorizontal();
            {
                m_SelectedAsset = EditorGUILayout.ObjectField("Selected Asset", m_SelectedAsset, typeof(UnityObject), false);

                if(GUILayout.Button("Search ...",GUILayout.Width(80)))
                {
                    RefreshTreeView();
                }
                EditorGUI.BeginDisabledGroup(m_ToolbarSelectedIndex != 1);
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
                        foreach (var extension in m_IngoreAssetExtensions)
                        {
                            extension.isSelected = EditorGUILayout.Toggle(extension.displayName, extension.isSelected);
                        }
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_TreeView.GetModel<AssetDependencyTreeViewModel>().SetIgnoreExtension(GetSelectedIgnoreExtensions());
                    }
                }
                EGUI.EndIndent();
            }
            EditorGUILayout.EndVertical();
        }

        private string[] GetSelectedIgnoreExtensions()
        {
            return (from extension in m_IngoreAssetExtensions
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
