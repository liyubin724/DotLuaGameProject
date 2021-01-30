using System.Collections.Generic;
using System.IO;

namespace DotEditor.AssetChecker
{
    [MatchFilter("File", "Extension")]
    public class FileExtensionMatchFilter : MatchFilter
    {
        public bool ignoreCase = true;
        public List<string> extensions = new List<string>();

        protected override bool MatchAsset(string assetPath)
        {
            string ext = Path.GetExtension(assetPath);
            if(string.IsNullOrEmpty(ext) || extensions.Count == 0)
            {
                return false;
            }
            foreach(var extension in extensions)
            {
                if(ignoreCase ? ext.ToLower() == extension.ToLower() : ext == extension)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void CloneTo(MatchFilter filter)
        {
            FileExtensionMatchFilter femf = filter as FileExtensionMatchFilter;
            femf.ignoreCase = ignoreCase;
            foreach(var ext in extensions)
            {
                femf.extensions.Add(ext);
            }
        }
    }
}
