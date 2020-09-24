using UnityEditor;

namespace DotEditor.Asset.Dependency
{
    internal class AssetDependOnTab : AAssetDependencyTab
    {
        public AssetDependOnTab(EditorWindow win) : base(win)
        {
        }

        protected override void OnAssetSelectionChanged(string assetPath)
        {
            treeView?.ShowDependency(new string[] { assetPath });
        }
    }
}
