using System.Collections.Generic;
using SystemObject = System.Object;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class TreeViewData
    {
        public int ID { get; internal set; } = -1;
        public int Depth { get; internal set; } = -1;
        public SystemObject UserData { get; set; } = null;
        public string DisplayName { get; set; } = string.Empty;

        internal bool IsExpand { get; set; } = false;
        internal TreeViewData Parent { get; set; } = null;
        internal List<TreeViewData> Children { get;} = new List<TreeViewData>();

        public virtual string GetDisplayName()
        {
            return GetType().Name;
        }

        public T GetData<T>()
        {
            return (T)UserData;
        }
    }
}
