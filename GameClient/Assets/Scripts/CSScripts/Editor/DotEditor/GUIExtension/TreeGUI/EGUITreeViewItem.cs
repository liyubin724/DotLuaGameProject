using UnityEditor.IMGUI.Controls;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class EGUITreeViewItem : TreeViewItem
    {
        public TreeViewData ItemData { get; private set; }

        public EGUITreeViewItem(TreeViewData data)
        {
            ItemData = data;

            id = data.ID;
            depth = data.Depth;
            displayName = data.DisplayName;
        }
    }
}
