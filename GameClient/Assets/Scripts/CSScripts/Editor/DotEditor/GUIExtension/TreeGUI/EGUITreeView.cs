using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class EGUITreeView : TreeView
    {
        public TreeViewModel TreeViewModel { get; private set; }
        private List<int> m_ExpandIDs = new List<int>();
        public EGUITreeView(TreeViewState state,TreeViewModel model) : base(state)
        {
            TreeViewModel = model;
            showBorder = true;
            showAlternatingRowBackgrounds = true;

            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            return new EGUITreeViewItem(TreeViewModel.RootData);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            root.children?.Clear();

            var rootData = ((EGUITreeViewItem)root).Data;

            TreeViewData[] childDatas = TreeViewModel.GetChilds(rootData);
            foreach (var childData in childDatas)
            {
                BuildItems((EGUITreeViewItem)root, childData);
            }
            return base.BuildRows(root);
        }

        private void BuildItems(EGUITreeViewItem parentItem, TreeViewData data)
        {
            EGUITreeViewItem item = new EGUITreeViewItem(data);
            if(parentItem.children == null)
            {
                parentItem.children = new List<TreeViewItem>();
            }
            parentItem.children.Add(item);

            if(data.IsExpand)
            {
                if(TreeViewModel.HasChild(data))
                {
                    foreach (var childData in data.Children)
                    {
                        BuildItems(item, childData);
                    }
                }
            }else
            {
                if(TreeViewModel.HasChild(data))
                {
                    item.children = TreeView.CreateChildListForCollapsedParent();
                }
            }
        }

        protected override void ExpandedStateChanged()
        {
            List<int> expandedIDs = state.expandedIDs;

            int[] exceptIDs = m_ExpandIDs.Except(expandedIDs).ToArray();
            TreeViewModel.CollapseDatas(exceptIDs);

            exceptIDs = expandedIDs.Except(m_ExpandIDs).ToArray();
            TreeViewModel.ExpandDatas(exceptIDs);

            m_ExpandIDs.Clear();
            m_ExpandIDs.AddRange(expandedIDs);

            base.ExpandedStateChanged();
            Reload();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (EGUITreeViewItem)args.item;
            Rect contentRect = args.rowRect;
            contentRect.x += GetContentIndent(item);
            contentRect.width -= GetContentIndent(item);

            DrawTreeViewItem(contentRect, item);
        }

        protected virtual void DrawTreeViewItem(Rect rect,EGUITreeViewItem item)
        {
            EditorGUI.LabelField(rect, item.Data.GetDisplayName());
        }
    }
}
