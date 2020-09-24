using DotEditor.GUIExtension;
using DotEditor.TreeGUI;
using DotEditor.Utilities;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.Dependency
{
    internal class TreeViewData
    {
        public AssetDependencyData dependencyData = null;

        public static TreeViewData Root
        {
            get => new TreeViewData();
        }
    }

    internal class AssetDependencyTreeView : TreeViewWithTreeModel<TreeElementWithData<TreeViewData>>
    {
        private int m_TreeViewElementIndex = 0;
        private Dictionary<int, string> m_ElementToAssetPathDic = new Dictionary<int, string>();
        private string[] m_IgnoreAssetExtensions = new string[0];
        private string[] m_MainAssetPaths = new string[0];
        private string[] m_SelectedAssetPaths = new string[0];

        public AssetDependencyTreeView(TreeViewState state, TreeModel<TreeElementWithData<TreeViewData>> model) : base(state, model)
        {
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            rowHeight = 32;
            
            Reload();
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return true;
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return false;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<TreeElementWithData<TreeViewData>>)args.item;
            Rect contentRect = args.rowRect;
            contentRect.x += GetContentIndent(item);
            contentRect.width -= GetContentIndent(item);
            contentRect.y += 2;
            contentRect.height -= 4;

            Rect iconRect = new Rect(contentRect.x, contentRect.y, contentRect.height, contentRect.height);
            AssetDependencyData adData = item.data.Data.dependencyData;
            UnityObject assetObj = AssetDatabase.LoadAssetAtPath(adData.assetPath, typeof(UnityObject));
            Texture2D previewIcon = null;
            if (!AssetPreview.IsLoadingAssetPreview(assetObj.GetInstanceID()))
            {
                previewIcon = AssetPreview.GetAssetPreview(assetObj);
            }
            if (previewIcon == null)
            {
                previewIcon = EGUIResources.MakeColorTexture((int)iconRect.width, (int)iconRect.height, Color.grey);
            }
            GUI.DrawTexture(iconRect, previewIcon, ScaleMode.ScaleAndCrop);
            if(Event.current.type == EventType.MouseUp && iconRect.Contains(Event.current.mousePosition))
            {
                SelectionUtility.PingObject(assetObj);
                SetSelection(new int[] { item.id }, TreeViewSelectionOptions.RevealAndFrame);
            }

            Rect labelRect = new Rect(iconRect.x + iconRect.width, iconRect.y, contentRect.width - iconRect.width, iconRect.height);
            EditorGUI.LabelField(labelRect, adData.assetPath, EGUIStyles.MiddleLeftLabelStyle);

            if(assetObj is Texture)
            {
                Rect memorySizeRect = new Rect(contentRect.x + contentRect.width -60, contentRect.y, 60, contentRect.height);
                long memorySize = AssetDatabaseUtility.GetTextureStorageSize(assetObj as Texture);
                EditorGUI.LabelField(memorySizeRect, EditorUtility.FormatBytes(memorySize));
            }
        }

        internal void SetIgnoreAssets(string[] ignoreExt)
        {
            m_IgnoreAssetExtensions = ignoreExt;
            if(treeModel.root.children != null && treeModel.root.children.Count>0)
            {
                ShowDependency(m_MainAssetPaths, m_SelectedAssetPaths);
            }
        }

        private bool IsIgnoreAsset(string assetPath)
        {
            if(m_IgnoreAssetExtensions == null || m_IgnoreAssetExtensions.Length == 0)
            {
                return false;
            }
            var extension = Path.GetExtension(assetPath).ToLower();

            return Array.IndexOf(m_IgnoreAssetExtensions, extension) >= 0;
        }

        public void RefreshDependency()
        {
            ShowDependency(m_MainAssetPaths, m_SelectedAssetPaths);
        }

        internal void ShowDependency(string[] assetPaths, string[] selectedAssetPaths = null)
        {
            m_MainAssetPaths = assetPaths;
            m_SelectedAssetPaths = selectedAssetPaths;

            m_ElementToAssetPathDic.Clear();
            treeModel.root.children?.Clear();

            if(assetPaths!=null && assetPaths.Length>0)
            {
                foreach(var assetPath in assetPaths)
                {
                    CreateDependencyChildNode(treeModel.root, assetPath);
                }
            }
            //ExpandAll();
            
            if(selectedAssetPaths!=null && selectedAssetPaths.Length>0)
            {
                int[] selectedItemIds = (from d in m_ElementToAssetPathDic where Array.IndexOf(selectedAssetPaths, d.Value) >= 0 select d.Key).ToArray();
                SetSelection(selectedItemIds, TreeViewSelectionOptions.RevealAndFrame);
            }
            SetFocus();
            Reload();
        }

        private void CreateDependencyChildNode(TreeElementWithData<TreeViewData> parentNode, string assetPath)
        {
            if(IsIgnoreAsset(assetPath))
            {
                return;
            }

            AssetDependencyData data = AssetDependencyUtil.GetDependencyData(assetPath);
            TreeElementWithData<TreeViewData> elementData = new TreeElementWithData<TreeViewData>(new TreeViewData()
            {
                dependencyData = data,
            }, data.assetPath, parentNode.depth + 1, m_TreeViewElementIndex);

            m_ElementToAssetPathDic.Add(elementData.id, elementData.name);

            m_TreeViewElementIndex++;
            treeModel.AddElement(elementData, parentNode, parentNode.hasChildren ? parentNode.children.Count : 0);

            if (data.directlyDepends != null && data.directlyDepends.Length > 0)
            {
                foreach (var path in data.directlyDepends)
                {
                    CreateDependencyChildNode(elementData, path);
                }
            }
        }
    }
}
