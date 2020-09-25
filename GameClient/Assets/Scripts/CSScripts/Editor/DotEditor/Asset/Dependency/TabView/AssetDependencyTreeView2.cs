using DotEditor.GUIExtension;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.Dependency
{
    internal class AssetDependencyTreeViewItem : TreeViewItem
    {
        public AssetDependencyData dependencyData = null;

        public static AssetDependencyTreeViewItem Root
        {
            get => new AssetDependencyTreeViewItem()
            {
                id = -1,
                depth = -1,
                displayName = "",
            };
        }
    }

    public class AssetDependencyTreeView2 : TreeView
    {
        private int m_TreeViewItemIndex = 0;
        private string[] m_MainAssetPaths = new string[0];
        private string[] m_SelectedAssetPaths = new string[0];

        public AssetDependencyTreeView2(TreeViewState state) : base(state)
        {
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            rowHeight = 32;
        }

        protected override TreeViewItem BuildRoot()
        {
            return AssetDependencyTreeViewItem.Root;
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            if (m_MainAssetPaths != null && m_MainAssetPaths.Length > 0)
            {
                foreach (var assetPath in m_MainAssetPaths)
                {
                    AssetDependencyTreeViewItem item = new AssetDependencyTreeViewItem()
                    {
                        id = m_TreeViewItemIndex++,
                        displayName = assetPath,
                        dependencyData = AssetDependencyUtil.GetDependencyData(assetPath),
                    };
                    item.children = TreeView.CreateChildListForCollapsedParent();
                    root.AddChild(item);
                }
            }
            SetupDepthsFromParentsAndChildren(root);
            return base.BuildRows(root);
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (AssetDependencyTreeViewItem)args.item;
            Rect contentRect = args.rowRect;
            contentRect.x += GetContentIndent(item);
            contentRect.width -= GetContentIndent(item);
            contentRect.y += 2;
            contentRect.height -= 4;

            Rect iconRect = new Rect(contentRect.x, contentRect.y, contentRect.height, contentRect.height);
            AssetDependencyData adData = item.dependencyData;
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
            if (Event.current.type == EventType.MouseUp && iconRect.Contains(Event.current.mousePosition))
            {
                SelectionUtility.PingObject(assetObj);
                SetSelection(new int[] { item.id }, TreeViewSelectionOptions.RevealAndFrame);
            }

            Rect labelRect = new Rect(iconRect.x + iconRect.width, iconRect.y, contentRect.width - iconRect.width, iconRect.height);
            EditorGUI.LabelField(labelRect, adData.assetPath, EGUIStyles.MiddleLeftLabelStyle);

            if (assetObj is Texture)
            {
                Rect memorySizeRect = new Rect(contentRect.x + contentRect.width - 60, contentRect.y, 60, contentRect.height);
                long memorySize = AssetDatabaseUtility.GetTextureStorageSize(assetObj as Texture);
                EditorGUI.LabelField(memorySizeRect, EditorUtility.FormatBytes(memorySize));
            }
        }

        internal void ShowDependency(string[] assetPaths, string[] selectedAssetPaths = null)
        {
            m_MainAssetPaths = assetPaths;
            m_SelectedAssetPaths = selectedAssetPaths;

            SetFocus();
            Reload();
        }
    }
}
