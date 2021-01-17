using System.IO;

namespace DotEditor.AssetChecker
{
    public class FileExtensionMatchFilter : MatchFilter
    {
        public bool IgnoreCase { get; set; } = true;
        public string Extension { get; set; }

        public override bool IsMatch(string assetPath)
        {
            if(string.IsNullOrEmpty(Extension))
            {
                return false;
            }
            string ext = Path.GetExtension(assetPath);
            return IgnoreCase ? ext.ToLower() == Extension.ToLower() : ext == Extension;
        }
    }
}
