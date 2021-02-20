using DotEditor.GUIExt.DataGrid;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS
{
    internal class AssetGridView : EGUIGridView 
    {
        internal AssetGridView():base(new GridViewModel(),new string[] { "Path","Address","Labels","Used Address"})
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
            if(columnIndex == 0)
            {
                EditorGUI.TextField(rect, data.path);
            }else if(columnIndex == 1)
            {
                EditorGUI.TextField(rect, data.address);
            }else if(columnIndex == 2)
            {
                EditorGUI.TextField(rect, (data.labels == null || data.labels.Length == 0) ? string.Empty : string.Join(",", data.labels));
            }else if(columnIndex == 3)
            {
                EditorGUI.Toggle(rect, data.usedAsAddress);
            }
        }
    }
}
