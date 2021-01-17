using System;
using System.Collections.Generic;
using System.IO;

namespace DotEditor.AssetChecker
{
    [MatchFilter("Match Filter/Folder", "In Any Folder")]
    public class InAnyFolderMatchFilter : MatchFilter
    {
        public bool IgnoreCase { get; set; } = true;
        public List<string> FolderNames { get; } = new List<string>();

        public override bool IsMatch(string assetPath)
        {
            if(FolderNames.Count == 0)
            {
                return false;
            }
            string assetDir = Path.GetDirectoryName(assetPath);
            string[] splitFolderNames = assetDir.Split(new char[] { '/' },StringSplitOptions.RemoveEmptyEntries);
            if(splitFolderNames!=null && splitFolderNames.Length>0)
            {
                foreach(var folderName in splitFolderNames)
                {
                    if(FolderNames.IndexOf(folderName)>=0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
