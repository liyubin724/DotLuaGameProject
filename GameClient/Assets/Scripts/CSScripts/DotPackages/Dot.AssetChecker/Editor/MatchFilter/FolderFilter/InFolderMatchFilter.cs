using System.IO;

namespace DotEditor.AssetChecker
{
    [MatchFilter("Match Filter/Folder", "In Folder")]
    public class InFolderMatchFilter : MatchFilter
    {
        public bool IsIgnoreCase { get; set; } = true;
        public string ParentFolderName { get; set; } = null;

        public override bool IsMatch(string assetPath)
        {
            if(string.IsNullOrEmpty(ParentFolderName))
            {
                return false;
            }
            string assetDir = Path.GetDirectoryName(assetPath);
            return IsIgnoreCase ? (ParentFolderName.ToLower() == assetDir.ToLower()) : ParentFolderName == assetDir;
        }
    }
}
