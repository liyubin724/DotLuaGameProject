using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.TreeGUI
{
    public delegate void SimpleListViewSelectedChange(int index);
    public delegate void SimpleListViewDrawItem(Rect rect, int index);
    public delegate float SimpleListViewGetItemHeight(int index);

    public class SimpleListView<T>
    {
        public SimpleListViewSelectedChange OnSelectedChange { get; set; }
        public SimpleListViewDrawItem OnDrawItem { get; set; }
        public SimpleListViewGetItemHeight GetItemHeight { get; set; }

        public string Header { get; set; } = null;
        public bool ShowSeparator { get; set; } = true;

        private List<T> itemDatas = new List<T>();

        private SimpleListTreeView<T> listTreeView;

        public SimpleListView()
        {
            listTreeView = new SimpleListTreeView<T>(this);
        }

        public int GetCount()
        {
            return itemDatas.Count;
        }

        public void AddItem(T itemData)
        {
            itemDatas.Add(itemData);

            listTreeView.Reload();
        }

        public void AddItems(T[] itemDatas)
        {
            foreach(var itemData in itemDatas)
            {
                this.itemDatas.Add(itemData);
            }
            listTreeView.Reload();
        }

        public void InsertItem(int index,T itemData)
        {
            itemDatas.Insert(index, itemData);

            listTreeView.Reload();
        }

        public void RemoveItem(T itemData)
        {
            itemDatas.Remove(itemData);

            listTreeView.Reload();
        }

        public void RemoveAt(int index)
        {
            if(index>=0 && index<itemDatas.Count)
            {
                itemDatas.RemoveAt(index);

                listTreeView.Reload();
            }
        }

        public T GetItem(int index)
        {
            if (index >= 0 && index < itemDatas.Count)
            {
                return itemDatas[index];
            }

            return default;
        }

        public void Clear()
        {
            itemDatas.Clear();
            Reload();
        }

        public void SetSelection(int index)
        {
            listTreeView.SetSelection(new int[] { index }, TreeViewSelectionOptions.FireSelectionChanged);
        }

        public void Reload()
        {
            listTreeView.Reload();
        }

        public void OnGUI(Rect rect)
        {
            listTreeView.OnGUI(rect);
        }
    }

    class SimpleListTreeViewItem<T> : TreeViewItem
    { 
        public static SimpleListTreeViewItem<T> DefaultRoot
        {
            get
            {
                return new SimpleListTreeViewItem<T>(-1,default);
            }
        }
        
        public T ItemData { get; private set; }

        public SimpleListTreeViewItem(int index,T itemData)
        {
            ItemData = itemData;

            id = index;
            if(id>=0)
            {
                depth = 0;
            }else
            {
                depth = -1;
            }
            displayName = ((object)itemData) != null ? itemData.ToString() : "";
            children = new List<TreeViewItem>();
        }
    }

    internal class SimpleListTreeView<T> : TreeView
    {
        private SimpleListView<T> listView = null;
        public SimpleListTreeView(SimpleListView<T> listView) : base(new TreeViewState())
        {
            this.listView = listView;

            showAlternatingRowBackgrounds = true;
            showBorder = true;
            useScrollView = true;

            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            SimpleListTreeViewItem<T> root = SimpleListTreeViewItem<T>.DefaultRoot;

            for(int i =0;i<listView.GetCount();++i)
            {
                SimpleListTreeViewItem<T> item = new SimpleListTreeViewItem<T>(i, listView.GetItem(i));
                root.AddChild(item);
            }

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Rect rect = args.rowRect;
            SimpleListTreeViewItem<T> viewItem = args.item as SimpleListTreeViewItem<T>;

            if (listView.ShowSeparator)
            {
                rect.height -= 6.0f;
            }

            if (listView.OnDrawItem == null)
            {
                EditorGUI.LabelField(rect, viewItem.displayName);
            }
            else
            {
                listView.OnDrawItem(rect, viewItem.id);
            }

            if (listView.ShowSeparator)
            {
                EGUI.DrawHorizontalLine(new Rect(rect.x, rect.y + rect.height, rect.width, 6.0f));
            }
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            float height = 0.0f;
            if (listView.GetItemHeight== null)
            {
                height = EditorGUIUtility.singleLineHeight;
            }
            else
            {
                height = listView.GetItemHeight(item.id);
            }
            if (listView.ShowSeparator)
            {
                height += 6.0f;
            }
            return height;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);

            if (selectedIds.Count > 0)
            {
                int selectedId = selectedIds[0];
                listView.OnSelectedChange?.Invoke(selectedId);
            }else
            {
                listView.OnSelectedChange?.Invoke(-1);
            }
        }

        public override void OnGUI(Rect rect)
        {
            Rect viewRect = rect;
            if (!string.IsNullOrEmpty(listView.Header))
            {
                EGUI.DrawBoxHeader(new Rect(rect.x, rect.y, rect.width, 30), listView.Header, EGUIStyles.BoxedHeaderCenterStyle);
                viewRect.y += 20;
                viewRect.height -= 20;
            }
            base.OnGUI(viewRect);
        }
    }
}
