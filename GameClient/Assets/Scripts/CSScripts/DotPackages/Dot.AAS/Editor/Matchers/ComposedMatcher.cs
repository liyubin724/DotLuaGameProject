using System.Collections.Generic;
using System;

namespace DotEditor.AAS.Matchers
{
    [Serializable]
    public class ComposedMatcher : IAssetMatcher
    {
        public List<IAssetMatcher> matchers = new List<IAssetMatcher>();
        public bool IsMatch(string assetPath)
        {
            if(matchers.Count == 0)
            {
                return false;
            }

            foreach(var m in matchers)
            {
                if(!m.IsMatch(assetPath))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
