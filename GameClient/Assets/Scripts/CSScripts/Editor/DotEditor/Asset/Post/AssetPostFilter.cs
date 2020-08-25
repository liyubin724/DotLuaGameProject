using DotEditor.Utilities;
using System;

namespace DotEditor.Asset.Post
{
    [Serializable]
    public class AssetPostFilter
    {
        public string Folder = "Assets";
        public bool IsIncludeSubfolder = true;
        public string FileNameRegex = string.Empty;

        public string[] GetResults()
        {
            return DirectoryUtility.GetAsset(Folder, IsIncludeSubfolder, FileNameRegex);
        }
    }
}
