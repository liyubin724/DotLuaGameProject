using DotEditor.GUIExt.DataGrid;
using DotEditor.Utilities;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS
{
    internal enum DataStatus
    {
        None = 0,
        Error,
        Warning,
    }

    internal class AssetGridViewData : GridViewData
    {
        public DataStatus status = DataStatus.None;
        public string statusTooltip = string.Empty;
        public string configAssetPath = string.Empty;
    }

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

        internal void SetDatas(List<AssetGridViewData> datas)
        {
            ViewModel.Clear();
            foreach(var data in datas)
            {
                ViewModel.AddData(data);
            }

            Reload();
        }

        protected override void OnDrawColumnItem(Rect rect, int columnIndex, GridViewData columnItemData)
        {
            AssetGridViewData viewData = columnItemData as AssetGridViewData;
            AssetBundleBuildData buildData = viewData.Userdata as AssetBundleBuildData;
            if(columnIndex == 0)
            {

            }else if(columnIndex == 1)
            {
                EditorGUI.TextField(rect, buildData.path);
            }else if(columnIndex == 2)
            {
                EditorGUI.TextField(rect, buildData.address);
            }else if(columnIndex == 3)
            {
                EditorGUI.TextField(rect, (buildData.labels == null || buildData.labels.Length == 0) ? string.Empty : string.Join(",", buildData.labels));
            }else if(columnIndex == 4)
            {
                EditorGUI.Toggle(rect, buildData.usedAsAddress);
            }
        }

        protected override void OnItemContextClicked(GridViewData itemData)
        {
            AssetGridViewData viewData = itemData as AssetGridViewData;
            AssetBundleBuildData buildData = viewData.Userdata as AssetBundleBuildData;

            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Selected Item"), false, () =>
            {
                SelectionUtility.ActiveObject(buildData.path);
            });
            menu.AddItem(new GUIContent("Selected Config"), false, () =>
              {
                  SelectionUtility.ActiveObject(viewData.configAssetPath);
              });
            menu.ShowAsContext();
        }
    }
}
