using System.Collections.Generic;
using SystemObject = System.Object;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class TreeViewData
    {
        public int ID { get; internal set; } = -1;
        public int Depth { get; internal set; } = -1;
        public SystemObject Userdata { get; set; } = null;
        public string DisplayName { get; set; } = string.Empty;

        internal bool IsExpand { get; set; } = false;
        internal TreeViewData Parent { get; set; } = null;
        internal List<TreeViewData> Children { get;} = new List<TreeViewData>();

        public TreeViewData(string displayName,SystemObject userdata = null)
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
