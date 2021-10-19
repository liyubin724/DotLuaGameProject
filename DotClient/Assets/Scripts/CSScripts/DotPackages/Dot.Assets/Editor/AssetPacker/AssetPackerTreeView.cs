using DotEditor.GUIExtension;
using DotEditor.TreeGUI;
using DotEditor.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Asset.AssetPacker
{
    public class AssetPackerTreeData
    {
        public int GroupIndex = -1;
        public int BundleIndex = -1;
        public int AssetIndex = -1;

        public bool IsGroup
        {
            get
            {
                return GroupIndex >= 0 && BundleIndex < 0 && AssetIndex < 0;
            }
        }

        public bool IsBundle
        {
            get
            {
                return GroupIndex >= 0 && BundleIndex >= 0 && AssetIndex < 0;
            }
        }

        public bool IsAsset
        {
            get
            {
                return GroupIndex >= 0 && BundleIndex >= 0 && AssetIndex >= 0;
            }
        }

        public static AssetPackerTreeData Root
        {
            get { return new AssetPackerTreeData(); }
        }
    }

    public class AssetPackerTreeView : TreeViewWithTreeModel<TreeElementWithData<AssetPackerTreeData>>
    {
        private const float SINGLE_ROW_HEIGHT = 17;

        private const int GROUP_ROW_HEIGHT = 20;
        private const int BUNDLE_ROW_HEIGHT = 20;
        private const int ASSET_ROW_HEIGHT = 40;

        public AssetPackerWindow Window { get; set; } = null;
        private GUIContent addressRepeatContent;
        public AssetPackerTreeView(TreeViewState state, TreeModel<TreeElementWithData<AssetPackerTreeData>> model)
            : base(state, model)
        {
            addressRepeatContent = EditorGUIUtility.IconContent("console.erroricon.sml", "Address Repeat");
            showBorder = true;
            showAlternatingRowBackgrounds = true;
            Reload();
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override bool CanStartDrag(CanStartDragArgs args)
        {
            return false;
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            var viewItem = (TreeViewItem<TreeElementWithData<AssetPackerTreeData>>)item;
            AssetPackerTreeData treeData = viewItem.data.Data;
            if(treeData.IsGroup)
            {
                return GROUP_ROW_HEIGHT;
            }
            if(treeData.IsBundle)
            {
                return BUNDLE_ROW_HEIGHT;
            }
            if(treeData.IsAsset)
            {
                return ASSET_ROW_HEIGHT;
            }
            return GROUP_ROW_HEIGHT;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<TreeElementWithData<AssetPackerTreeData>>)args.item;
            AssetPackerTreeData treeData = item.data.Data;

            int childCount = item.data.children == null ? 0 : item.data.children.Count;

            Rect contentRect = args.rowRect;
            contentRect.x += GetContentIndent(item);
            contentRect.width -= GetContentIndent(item);
            contentRect.y += 2;
            contentRect.height -= 4;

            if (treeData.IsGroup)
            {
                DrawGroupData(contentRect,Window.GetGroupData(treeData.GroupIndex), childCount);
            }
            else if(treeData.IsBundle)
            {
                DrawBundleData(contentRect, Window.GetBundleData(treeData.GroupIndex, treeData.BundleIndex),childCount);
            }else if(treeData.IsAsset)
            {
                DrawAddressData(contentRect, Window.GetAssetData(treeData.GroupIndex,treeData.BundleIndex,treeData.AssetIndex));
            }
        }

        private void DrawGroupData(Rect contentRect,PackerGroupData groupData,int childCount)
        {
            Rect drawRect = new Rect(contentRect.x, contentRect.y, contentRect.height, contentRect.height);
            drawRect.x += drawRect.width + 5;
            drawRect.width = contentRect.width - drawRect.x;

            EditorGUI.LabelField(drawRect,new GUIContent($"{groupData.GroupName}({childCount})"));
        }

        private void DrawBundleData(Rect contentRect,PackerBundleData bundleData, int childCount)
        {
            Rect drawRect = new Rect(contentRect.x, contentRect.y, contentRect.height, contentRect.height);
            drawRect.x += drawRect.width + 5;
            drawRect.width = contentRect.width - drawRect.x;

            EditorGUI.LabelField(drawRect, new GUIContent($"{bundleData.BundlePath}({childCount})"));
        }

        private void DrawAddressData(Rect contentRect, PackerAssetData assetData)
        {
            EGUI.DrawAreaLine(contentRect, Color.black);

            Rect pingBtnRect = new Rect(contentRect.x + contentRect.width - contentRect.height, contentRect.y, contentRect.height, contentRect.height);
            if (GUI.Button(pingBtnRect, "Select"))
            {
                SelectionUtility.PingObject(assetData.Path);
            }

            Rect ValueRect = new Rect(contentRect.x, contentRect.y, contentRect.width - contentRect.height, contentRect.height);
            EGUI.DrawAreaLine(contentRect, Color.grey);

            Rect drawRect = new Rect(ValueRect.x, ValueRect.y, ValueRect.width*0.5f, ValueRect.height*0.5f);
            EGUI.BeginLabelWidth(80);
            {
                EditorGUI.TextField(drawRect, "path", assetData.Path);
            }
            EGUI.EndLableWidth();

            drawRect = new Rect(ValueRect.x, ValueRect.y + drawRect.height, ValueRect.width * 0.5f, ValueRect.height * 0.5f);
            EGUI.BeginLabelWidth(80);
            {
                EditorGUI.TextField(drawRect, "address", assetData.Address);
            }
            EGUI.EndLableWidth();

            drawRect = new Rect(ValueRect.x + ValueRect.width * 0.5f, ValueRect.y, ValueRect.width * 0.5f, ValueRect.height * 0.5f);
            EGUI.BeginLabelWidth(80);
            {
                EditorGUI.TextField(drawRect, "labels", string.Join(",", assetData.Labels));
            }
            EGUI.EndLableWidth();

            drawRect = new Rect(ValueRect.x + ValueRect.width * 0.5f, ValueRect.y + drawRect.height, ValueRect.width * 0.5f, ValueRect.height * 0.5f);
            EGUI.BeginLabelWidth(80);
            {
                
            }
            EGUI.EndLableWidth();
            //if (Window.IsAddressRepeated(assetData.Address, out List<PackerBundleData> datas))
            //{
            //    if (GUI.Button(drawRect,addressRepeatContent))
            //    {
            //        AssetAddressRepeatPopupContent content = new AssetAddressRepeatPopupContent()
            //        {
            //            RepeatAddressDatas = datas.ToArray(),
            //        };
            //        PopupWindow.Show(drawRect, content);
            //    }
            //}

        }
    }
}
