using DotEditor.GUIExtension.DataGrid;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Monitor
{
    public class ProfilerTabTreeView : EGUIGridView
    {
        public ProfilerTabTreeView(ProfilerTabModel model,string[] titles) : base(model,titles)
        {
        }

        protected override void OnDrawColumnItem(Rect rect, int columnIndex, GridViewData columnItemData)
        {
            string title = ViewHeader.GetColumnTitle(columnIndex);
            PropertyInfo pInfo = columnItemData.Userdata.GetType().GetProperty(title, BindingFlags.Instance | BindingFlags.Public);
            string content = string.Empty;
            if (pInfo != null)
            {
                content = pInfo.GetValue(columnItemData.Userdata).ToString();
            }
            EditorGUI.LabelField(rect, content);
        }
    }
}
