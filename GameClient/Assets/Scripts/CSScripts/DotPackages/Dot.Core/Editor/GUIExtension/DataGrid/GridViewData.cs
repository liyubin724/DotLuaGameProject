using System.Collections.Generic;
using SystemObject = System.Object;

namespace DotEditor.GUIExtension.DataGrid
{
    public class GridViewData
    {
        public int ID { get; internal set; } = -1;
        public int Depth { get; internal set; } = -1;
        public SystemObject Userdata { get; set; } = null;
        public string DisplayName { get; set; } = string.Empty;

        public bool IsExpand { get; set; } = false;
        public GridViewData Parent { get; set; } = null;
        public List<GridViewData> Children { get; } = new List<GridViewData>();

        public GridViewData(string displayName, SystemObject userdata = null)
        {
            DisplayName = displayName;
            Userdata = userdata;
        }

        public T GetData<T>()
        {
            return (T)Userdata;
        }
    }
}
