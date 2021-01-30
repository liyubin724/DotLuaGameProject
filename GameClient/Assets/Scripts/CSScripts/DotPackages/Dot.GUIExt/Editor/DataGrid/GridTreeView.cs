using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExt.DataGrid
{
    public class GridTreeViewItem : TreeViewItem
    {
        public GridViewData ItemData { get; private set; }

        public GridTreeViewItem(GridViewData data)
        {
            ItemData = data;

            id = ItemData.ID;
            depth = ItemData.Depth;
            displayName = ItemData.DisplayName;
        }
    }

    public delegate void DrawRowItem(Rect rect, GridViewData itemData);
    public delegate void DrawColumnItem(Rect rect, int columnIndex, GridViewData itemData);
    public delegate float GetRowHeight(GridViewData itemData);
    public delegate void ItemDoubleClicked(GridViewData itemData);
    public delegate void ItemContextClicked(GridViewData itemData);
    public delegate void ItemSelectedChanged(GridViewData[] itemDatas);
    public delegate bool GetCanMultiSelect(GridViewData itemData);

    public class GridTreeView : TreeView
    {
        public GridViewModel ViewModel { get; private set; }

        public bool IsMultiSelect { get; set; } = false;

        public DrawRowItem OnDrawRowItem { get; set; }
        public DrawColumnItem OnDrawColumnItem { get; set; }
        public GetRowHeight OnGetRowHeight { get; set; }
        public ItemContextClicked OnItemContextClicked { get; set; }
        public ItemDoubleClicked OnItemDoubleClicked { get; set; }
        public ItemSelectedChanged OnItemSelectedChanged { get; set; }
        public GetCanMultiSelect GetCanMultiSelect { get; set; }

        private bool m_HasHeader = false;
        private List<int> m_ExpandIDs = new List<int>();
        public GridTreeView(TreeViewState state, GridViewModel model) : base(state)
        {
            ViewModel = model;

            showBorder = true;
            showAlternatingRowBackgrounds = true;
        }

        public GridTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader, GridViewModel model) : base(state, multiColumnHeader)
        {
            ViewModel = model;
            
            m_HasHeader = true;

            showBorder = true;
            showAlternatingRowBackgrounds = true;

            multiColumnHeader.ResizeToFit();
        }

        public T GetViewModel<T>() where T:GridViewModel
        {
            return (T)ViewModel;
        }

        protected override TreeViewItem BuildRoot()
        {
            return new GridTreeViewItem(ViewModel.RootData);
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            root.children?.Clear();

            GridTreeViewItem rootItem = (GridTreeViewItem)root;
            GridViewData[] childDatas = ViewModel.GetChilds(rootItem.ItemData);
            foreach(var child in childDatas)
            {
                BuildItems(rootItem, child);
            }

            return base.BuildRows(root);
        }

        private void BuildItems(GridTreeViewItem parentItem,GridViewData data)
        {
            GridTreeViewItem item = new GridTreeViewItem(data);
            if(parentItem.children == null)
            {
                parentItem.children = new List<TreeViewItem>();
            }
            parentItem.children.Add(item);

            if(data.IsExpand)
            {
                if(ViewModel.HasChild(data))
                {
                    foreach(var child in data.Children)
                    {
                        BuildItems(item, child);
                    }
                }
            }else
            {
                if(ViewModel.HasChild(data))
                {
                    item.children = TreeView.CreateChildListForCollapsedParent();
                }
            }
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if(OnItemSelectedChanged!=null)
            {
                GridViewData[] datas = new GridViewData[selectedIds.Count];
                for(int i =0;i<selectedIds.Count;++i)
                {
                    datas[i] = ViewModel.GetDataByID(selectedIds[i]);
                }

                OnItemSelectedChanged(datas);
            }else
            {
                base.SelectionChanged(selectedIds);
            }
        }

        protected override void ExpandedStateChanged()
        {
            List<int> expandedIDs = state.expandedIDs;

            int[] exceptIDs = m_ExpandIDs.Except(expandedIDs).ToArray();
            ViewModel.CollapseDatas(exceptIDs);

            exceptIDs = expandedIDs.Except(m_ExpandIDs).ToArray();
            ViewModel.ExpandDatas(exceptIDs);

            m_ExpandIDs.Clear();
            m_ExpandIDs.AddRange(expandedIDs);

            base.ExpandedStateChanged();
        }

        protected override void DoubleClickedItem(int id)
        {
            GridViewData data = ViewModel.GetDataByID(id);
            OnItemDoubleClicked?.Invoke(data);
        }

        protected override void ContextClickedItem(int id)
        {
            GridViewData data = ViewModel.GetDataByID(id);
            OnItemContextClicked?.Invoke(data);
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            GridViewData data = ((GridTreeViewItem)item).ItemData;

            return OnGetRowHeight != null ? OnGetRowHeight(data) : base.GetCustomRowHeight(row, item);
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            if(IsMultiSelect && GetCanMultiSelect != null)
            {
                GridViewData data = ((GridTreeViewItem)item).ItemData;
                return GetCanMultiSelect(data);
            }
            else
            {
                return false;
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item = (GridTreeViewItem)args.item;
            if(!m_HasHeader)
            {
                Rect contentRect = args.rowRect;
                contentRect.x += GetContentIndent(item);
                contentRect.width -= GetContentIndent(item);

                if (OnDrawRowItem!=null)
                {
                    OnDrawRowItem(contentRect, item.ItemData);
                }else
                {
                    DrawRowItem(contentRect, item.ItemData);
                }
            }else
            {
                for(int i =0;i<args.GetNumVisibleColumns();++i)
                {
                    Rect cellRect = args.GetCellRect(i);
                    CenterRectUsingSingleLineHeight(ref cellRect);

                    int columnIndex = args.GetColumn(i);
                    if (OnDrawColumnItem != null)
                    {
                        OnDrawColumnItem(cellRect, columnIndex, item.ItemData);
                    }else
                    {
                        DrawCellItem(cellRect, columnIndex, item.ItemData);
                    }
                }
            }
        }

        protected virtual void DrawRowItem(Rect rect,GridViewData itemData)
        {
            EditorGUI.LabelField(rect, itemData.DisplayName);
        }

        protected virtual void DrawCellItem(Rect cellRect,int index,GridViewData itemData)
        {
            EditorGUI.LabelField(cellRect, itemData.DisplayName);
        }
    }
}
