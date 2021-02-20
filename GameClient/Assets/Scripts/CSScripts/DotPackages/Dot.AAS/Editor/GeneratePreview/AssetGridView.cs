using DotEditor.GUIExt.DataGrid;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS
{
    internal class AssetGridView : EGUIGridView 
    {
        internal AssetGridView() : base(new GridViewModel(), new GridViewColumn[] { 
            new GridViewColumn("Status")
            {
                Width = 60,
                MaxWidth = 60,
                MinWidth = 60,
                AutoResize = false,
            },
            new GridViewColumn("Path"),
            new GridViewColumn("Address"),
            new GridViewColumn("Labels"),
            new GridViewColumn("Used Address"){
                Width = 100,
                MaxWidth = 100,
                MinWidth = 100,
                AutoResize = false,
            },
        })
        {
        }

        internal void SetDatas(List<AssetBundleBuildData> list)
        {
            ViewModel.Clear();
            foreach(var data in list)
            {
                ViewModel.AddData(new GridViewData(data.path, data));
            }
            Reload();
        }

        protected override void OnDrawColumnItem(Rect rect, int columnIndex, GridViewData columnItemData)
        {
            AssetBundleBuildData data = columnItemData.GetData<AssetBundleBuildData>();
            if(columnIndex == 1)
            {
                EditorGUI.TextField(rect, data.path);
            }else if(columnIndex == 2)
            {
                EditorGUI.TextField(rect, data.address);
            }else if(columnIndex == 3)
            {
                EditorGUI.TextField(rect, (data.labels == null || data.labels.Length == 0) ? string.Empty : string.Join(",", data.labels));
            }else if(columnIndex == 4)
            {
                EditorGUI.Toggle(rect, data.usedAsAddress);
            }
        }
    }
}
