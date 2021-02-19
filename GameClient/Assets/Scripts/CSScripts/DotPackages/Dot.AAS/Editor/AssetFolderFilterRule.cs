using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.AAS
{
    public enum AssetFolderFilterType
    {
        None = 0,
    }

    [Serializable]
    public class AssetFolderFilterRule
    {
        public bool IsValid(string assetPath)
        {
            return true;
        }
    }
}
