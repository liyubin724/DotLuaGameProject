using UnityEditor.IMGUI.Controls;

namespace DotEditor.GUIExtension.TreeGUI
{
    public class EGUITreeViewItem : TreeViewItem
    {
        public TreeViewData Data { get; private set; }

        public EGUITreeViewItem(TreeViewData data)
        {
            Data = data;

            id = data.ID;
            depth = data.Depth;
            displayName = data.GetDisplayName();
        }
    }
}
