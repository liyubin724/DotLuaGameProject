using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.DataGrid
{
    public class EGUIListView<T> : EGUITreeView where T:class
    {
        public string Header { get; set; } = null;
        public bool ShowSeparator { get; set; } = true;

        public EGUIListView() : base()
        {
        }

        public void AddItem(T data)
        {
            ViewModel.AddData(new GridViewData(data.ToString(), data));
            Reload();
        }

        public void AddItems(T[] datas)
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

        public void RemoveItem(T data)
        {
            GridViewData viewData = ViewModel.GetDataByUserdata(data);
            if(viewData!=null)
            {
                ViewModel.RemoveData(viewData);

                Reload();
            }
        }

    }
}
