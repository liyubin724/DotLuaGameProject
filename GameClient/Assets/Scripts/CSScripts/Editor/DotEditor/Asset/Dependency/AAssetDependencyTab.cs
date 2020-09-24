using DotEditor.GUIExtension;
using DotEditor.TreeGUI;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.Dependency
{
    internal abstract class AAssetDependencyTab
    {
        protected EditorWindow window = null;

        protected AssetDependencyTreeView treeView = null;
        private TreeViewState m_TreeViewState = null;

        private UnityObject m_SelectedAsset = null;
        private IngoreAssetExtension[] m_IngoreAssetExtensions = new IngoreAssetExtension[]
        {
            new IngoreAssetExtension()
            {
                displayName = "txt",
                extension = ".txt",
                isSelected = false,
            },
            new IngoreAssetExtension()
            {
                displayName = "cs",
                extension = ".cs,.dll",
                isSelected = false,
            },
            new IngoreAssetExtension()
            {
                displayName = "lua",
                extension = ".txt,.lua",
                isSelected = false,
            },
            new IngoreAssetExtension()
            {
                displayName = "Shader",
                extension = ".shader,.cginc",
                isSelected = false,
            },
        };

        public AAssetDependencyTab(EditorWindow win)
        {
            window = win;
        }

        public virtual void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            if (Selection.activeObject != null
                && Selection.activeObject != m_SelectedAsset
                && Selection.activeObject.GetType() != typeof(DefaultAsset)
                && EditorUtility.IsPersistent(Selection.activeObject))
            {
                m_SelectedAsset = Selection.activeObject;
                OnAssetSelectionChanged(AssetDatabase.GetAssetPath(m_SelectedAsset));
            }
        }

        public virtual void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
        }
        
        public virtual void OnGUI()
        {
            if (treeView == null)
            {
                InitTreeView();
            }
            DrawSelectedAsset();
            DrawIngoreAssetExtension();
            DrawTreeView();
        }

        protected abstract void OnAssetSelectionChanged(string assetPath);

        protected virtual void OnInitedTreeView()
        {
        }

        private void InitTreeView()
        {
            m_TreeViewState = new TreeViewState();
            TreeModel<TreeElementWithData<TreeViewData>> model = new TreeModel<TreeElementWithData<TreeViewData>>(
                new List<TreeElementWithData<TreeViewData>>()
                {
                    new TreeElementWithData<TreeViewData>(TreeViewData.Root,"",-1,-1)
                });
            treeView = new AssetDependencyTreeView(m_TreeViewState, model);

            OnInitedTreeView();
        }

        protected virtual void DrawSelectedAsset()
        {
            UnityObject asset = EditorGUILayout.ObjectField("Selected Asset", m_SelectedAsset, typeof(UnityObject), false);
            if (asset != m_SelectedAsset)
            {
                m_SelectedAsset = asset;
                OnAssetSelectionChanged(AssetDatabase.GetAssetPath(m_SelectedAsset));
            }
        }

        protected virtual void DrawIngoreAssetExtension()
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
                        string[] selectedExtensions = (from extension in m_IngoreAssetExtensions
                                                       where extension.isSelected
                                                       let exts = extension.extension.Split(new char[] { ',' })
                                                       from ext in exts
                                                       select ext
                                                       ).ToArray();
                        treeView.SetIgnoreAssets(selectedExtensions);
                    }
                }
                EGUI.EndIndent();
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawTreeView()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar,GUILayout.ExpandWidth(true));
            {
                if(GUILayout.Button("Expand",EditorStyles.toolbarButton,GUILayout.Width(120)))
                {
                    treeView.ExpandAll();
                }
                if (GUILayout.Button("Collapse", EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    treeView.CollapseAll();
                }
            }
            EditorGUILayout.EndHorizontal();

            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            treeView.OnGUI(rect);
        }
    }
}
