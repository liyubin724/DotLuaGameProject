using System;
using UnityEditor;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEditor.GUIExtension.DataGrid
{
    public class EGUISampleListView: EGUITreeView
    {
        private static float SEPARATOR_LINE_HEIGHT = 6.0f;

        public bool ShowSeparator { get; set; } = true;

        public Action<SystemObject> OnSelectedChange { get; set; }

        public EGUISampleListView() : base()
        {
        }

        public void AddItem (SystemObject data)
        {
            ViewModel.AddData(new GridViewData(data.ToString(), data));
            Reload();
        }

        public void InsertItem(int index,SystemObject data)
        {
            ViewModel.InsertData(index, new GridViewData(data.ToString(), data));
            Reload();
        }

        public void AddItems(SystemObject[] datas)
        {
            if(datas!=null && datas.Length>0)
            {
                foreach(var data in datas)
                {
                    ViewModel.AddData(new GridViewData(data.ToString(), data));
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

        public SystemObject GetItem(int index)
        {
            var rootChilds = ViewModel.RootData.Children;
            if (index >= 0 && index < rootChilds.Count)
            {
                var item = rootChilds[index];

                return item.Userdata;
            }
            return null;
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
            
            drawRect.y += drawRect.height;
            drawRect.height = SEPARATOR_LINE_HEIGHT;
            EGUI.DrawHorizontalLine(drawRect);
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
