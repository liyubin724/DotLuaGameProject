using DotEditor.TreeGUI;
using DotEditor.Utilities;
using DotEditor.GUIExtension;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.Asset.AssetPacker
{
    public class AssetPackerTreeData
    {
        public PackerGroupData groupData;
        public int dataIndex = -1;

        public bool IsGroup { get => dataIndex < 0; }

        public static AssetPackerTreeData Root
        {
            get { return new AssetPackerTreeData(); }
        }
    }

    public class AssetPackerTreeView : TreeViewWithTreeModel<TreeElementWithData<AssetPackerTreeData>>
    {
        private const float SINGLE_ROW_HEIGHT = 17;

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
            AssetPackerTreeData groupTreeData = viewItem.data.Data;
            if (groupTreeData.IsGroup)
            {
                return SINGLE_ROW_HEIGHT + 8;
            }
            else
            {
                return SINGLE_ROW_HEIGHT * 5;
            }
        }
        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (TreeViewItem<TreeElementWithData<AssetPackerTreeData>>)args.item;
            AssetPackerTreeData groupTreeData = item.data.Data;
            PackerGroupData groupData = groupTreeData.groupData;

            int childCount = item.data.children == null ? 0 : item.data.children.Count;

            Rect contentRect = args.rowRect;
            contentRect.x += GetContentIndent(item);
            contentRect.width -= GetContentIndent(item);
            contentRect.y += 2;
            contentRect.height -= 4;

            if (groupTreeData.IsGroup)
            {
                DrawGroupData(contentRect, args.item.id,groupData, childCount);
            }
            else
            {
                PackerBundleData addressData = groupData.assetFiles[groupTreeData.dataIndex];
                DrawAddressData(contentRect, addressData);
            }
        }

        private void DrawGroupData(Rect contentRect,int itemID,PackerGroupData groupData,int childCount)
        {
            Rect drawRect = new Rect(contentRect.x, contentRect.y, contentRect.height, contentRect.height);
            if (Window.IsGroupAddressRepeated(groupData))
            {
                if (GUI.Button(drawRect,addressRepeatContent))
                {
                    SetExpanded(itemID, true);
                }
            }

            drawRect.x += drawRect.width + 5;
            drawRect.width = contentRect.width - drawRect.x;

            string groupName = groupData.groupName;
            if (groupData.isMain)
            {
                groupName += "(Main)";
            }
            if (groupData.isPreload)
            {
                groupName += "(Preload)";
            }
            if (groupData.isNeverDestroy)
            {
                groupName += "(NeverDestroy)";
            }
            groupName += "  " + childCount;

            EditorGUI.LabelField(drawRect,new GUIContent(groupName));
        }

        private void DrawAddressData(Rect contentRect,PackerBundleData addressData)
        {
            Rect drawRect = new Rect(contentRect.x, contentRect.y, 24, 24);
            if (Window.IsAddressRepeated(addressData.Address, out List<PackerBundleData> datas))
            {
                if (GUI.Button(drawRect,addressRepeatContent))
                {
                    AssetAddressRepeatPopupContent content = new AssetAddressRepeatPopupContent()
                    {
                        RepeatAddressDatas = datas.ToArray(),
                    };
                    PopupWindow.Show(drawRect, content);
                }
            }

            drawRect.x += drawRect.width + 5;
            drawRect.width = (contentRect.width - drawRect.x - contentRect.height - 20)*.5f;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            EGUI.BeginLabelWidth(80);
            {
                EditorGUI.TextField(drawRect,"address", addressData.Address);
            }
            EGUI.EndLableWidth();

            drawRect.x += drawRect.width+5;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            EGUI.BeginLabelWidth(80);
            {
                EditorGUI.TextField(drawRect, "path", addressData.Path);
                drawRect.y += drawRect.height;
                EditorGUI.TextField(drawRect, "bundle", addressData.Bundle);
                drawRect.y += drawRect.height;
                EditorGUI.TextField(drawRect, "bundle-md5", addressData.bundlePathMd5);
                drawRect.y += drawRect.height;
                EditorGUI.TextField(drawRect, "labels", string.Join(",", addressData.Labels));
            }
            EGUI.EndLableWidth();

            drawRect = contentRect;
            drawRect.x += contentRect.width - contentRect.height - 20;
            drawRect.width = contentRect.height;
            drawRect.height = contentRect.height;

            if (GUI.Button(drawRect,"Select"))
            {
                SelectionUtility.PingObject(addressData.Path);
            }
        }
    }
}
