using DotEditor.GUIExtension.DataGrid;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Examples
{
    public class FileInfoData
    {
        public string path;
        public string createDatetime;
        public long fileSize;
    }
    public class FileShowGridView : EGUIGridView
    {
        public FileShowGridView(GridViewModel model, GridViewHeader header) : base(model, header)
        {
        }

        protected override void OnDrawColumnItem(Rect rect, int columnIndex, GridViewData columnItemData)
        {
            FileInfoData data = columnItemData.GetData<FileInfoData>();
            string content = string.Empty;
            if(columnIndex == 0)
            {
                content = data.path;
            }else if(columnIndex == 1)
            {
                content = data.createDatetime;
            }else
            {
                content = data.fileSize.ToString();
            }
            EditorGUI.LabelField(rect, content);
        }

    }
}
