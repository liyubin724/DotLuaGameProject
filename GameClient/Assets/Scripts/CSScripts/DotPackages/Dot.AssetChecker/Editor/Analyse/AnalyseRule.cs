using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    public abstract class AnalyseRule : IAnalyseRule
    {
        public bool enable = true;

        public abstract bool AnalyseAsset(UnityObject uObj, ref int errorCode);
    }
}
