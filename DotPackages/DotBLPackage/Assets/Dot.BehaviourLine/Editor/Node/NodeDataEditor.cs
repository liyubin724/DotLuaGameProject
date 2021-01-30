using DotEditor.NativeDrawer;
using DotEngine.BL.Node;
using UnityEditor;

namespace DotEditor.BL.Node
{
    [CustomEditor(typeof(NodeData),true,isFallback =true)]
    public class NodeDataEditor : NativeDrawerEditor
    {
    }
}
