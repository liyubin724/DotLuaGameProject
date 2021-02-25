using UnityEditor;

namespace DotEditor.AAS.Reprocessor
{
    public class PostprocessorHandle : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if(importedAssets!=null && importedAssets.Length>0)
            {
                AssetReprocessUtility.PostprocessImportAssets(importedAssets);
            }
        }
    }
}
