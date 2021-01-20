using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExt.DataGrid
{
    public class EasyListView: EGUITreeView
    {
        private static float SEPARATOR_LINE_HEIGHT = 6.0f;

        public bool ShowSeparator { get; set; } = true;

        public Action<SystemObject> OnSelectedChange { get; set; }
        public SystemObject SelectedItem
        {
            get
            {
                IList<int> selectedIDList = treeView.GetSelection();
                if(selectedIDList!=null && selectedIDList.Count>0)
                {
                    return GetItemByID(selectedIDList[0]);
                }

                return null;
            }
            set
            {
                if(value == null)
                {
                    treeView.SetSelection(new List<int>() { });
                }else
                {
                    int id = GetIDByItem(value);
                    if(id>=0)
                    {
                        treeView.SetSelection(new List<int>() { id }, TreeViewSelectionOptions.FireSelectionChanged);
                    }
                }
            }
        }

        public EasyListView() : base()
        {
        }

        public EasyListView(string[] displayNames,SystemObject[] datas):this()
        {
            for(int i =0;i<displayNames.Length;++i)
            {
                ViewModel.AddData(new GridViewData(displayNames[i], datas[i]));
            }
            Reload();
        }

        public void AddItem (string displayName,SystemObject data)
        {
            ViewModel.AddData(new GridViewData(displayName, data));
            Reload();
        }

        public void InsertItem(int index,string displayName, SystemObject data)
        {
            ViewModel.InsertData(index, new GridViewData(displayName, data));
            Reload();
        }

        public void AddItems(string[] displayNames,SystemObject[] datas)
        {
            if(datas!=null && datas.Length>0)
            {
                for (int i = 0; i < displayNames.Length; ++i)
                {
                    ViewModel.AddData(new GridViewData(displayNames[i], datas[i]));
                }

                Reload();
            }
        }

        public void RemoveItem(SystemObject data)
        {
            GridViewData viewData = ViewModel.GetDataByUserdata(data);
            if(viewData!=null)
            {
                ViewModel.RemoveData(viewData);

                Reload();
            }
        }

        public void RemoveAt(int index)
        {
            var rootChilds = ViewModel.RootData.Children;
            if(index>=0&&index < rootChilds.Count)
            {
                var item = rootChilds[index];
                ViewModel.RemoveData(item);

                Reload();
            }
        }

        private SystemObject GetItemByID(int id)
        {
            var rootChilds = ViewModel.RootData.Children;
            for (int i = 0; i < rootChilds.Count; ++i)
            {
                if (rootChilds[i].ID == id)
                {
                    return rootChilds[i].Userdata;
                }
            }

            return null;
        }

        private int GetIDByItem(SystemObject data)
        {
            var rootChilds = ViewModel.RootData.Children;
            for(int i =0;i<rootChilds.Count;++i)
            {
                if(rootChilds[i].Userdata == data)
                {
                    return rootChilds[i].ID;
                }
            }

            return -1;
        }

        public void Clear()
        {
            ViewModel.Clear();
            Reload();
        }

        protected override void OnDrawRowItem(Rect rect, GridViewData itemData)
        {
            Rect drawRect = rect;
            if(ShowSeparator)
            {
                drawRect.height -= SEPARATOR_LINE_HEIGHT;
            }
            EditorGUI.LabelField(drawRect, itemData.DisplayName);

            if (ShowSeparator)
            {
                drawRect.y += drawRect.height;
                drawRect.height = SEPARATOR_LINE_HEIGHT;
                EGUI.DrawHorizontalLine(drawRect);
            }
        }

        protected override float GetRowHeight(GridViewData itemData)
        {
            return base.GetRowHeight(itemData) + (ShowSeparator? SEPARATOR_LINE_HEIGHT : 0.0f);
        }

        protected override void OnItemSelectedChanged(GridViewData[] itemDatas)
        {
            if (itemDatas != null && itemDatas.Length > 0)
            {
                OnSelectedChange?.Invoke(itemDatas[0].Userdata);
            }
        }
    }
}
