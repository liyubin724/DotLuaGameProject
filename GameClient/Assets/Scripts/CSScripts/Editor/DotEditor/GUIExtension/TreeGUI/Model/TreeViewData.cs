using System.Collections.Generic;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class TreeViewData
    {
        internal int ID { get; set; } = -1;
        internal int Depth { get; set; } = -1;

        internal bool IsExpand { get; set; } = false;
        internal TreeViewData Parent { get; set; } = null;
        internal List<TreeViewData> Children { get;} = new List<TreeViewData>();

        public virtual string GetDisplayName()
        {
            return GetType().Name;
        }
    }
}
