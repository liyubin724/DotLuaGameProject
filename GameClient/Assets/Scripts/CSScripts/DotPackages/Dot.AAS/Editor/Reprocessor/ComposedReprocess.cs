using System;
using System.Collections.Generic;

namespace DotEditor.AAS.Reprocessor
{
    [Serializable]
    public class ComposedReprocess : IAssetReprocess
    {
        public List<IAssetReprocess> reprocesses = new List<IAssetReprocess>();

        public void Execute(string assetPath)
        {
            foreach(var r in reprocesses)
            {
                r.Execute(assetPath);
            }
        }
    }
}
