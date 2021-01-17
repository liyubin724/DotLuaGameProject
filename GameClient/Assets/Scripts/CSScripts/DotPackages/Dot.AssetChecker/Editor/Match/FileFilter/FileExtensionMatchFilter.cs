using System.IO;

namespace DotEditor.AssetChecker
{
    public class FileExtensionMatchFilter : MatchFilter
    {
        public bool ignoreCase = true;
        public string extension = null;

        protected override void CloneTo(MatchFilter filter)
        {
            FileExtensionMatchFilter femf = filter as FileExtensionMatchFilter;
            femf.ignoreCase = ignoreCase;
            femf.extension = extension;
        }

        protected override bool MatchAsset(string assetPath)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }
            string ext = Path.GetExtension(assetPath);
            return ignoreCase ? ext.ToLower() == extension.ToLower() : ext == extension;
        }
    }
}
