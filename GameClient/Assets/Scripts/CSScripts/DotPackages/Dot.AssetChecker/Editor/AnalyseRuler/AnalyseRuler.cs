using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    public abstract class AnalyseRuler : IAnalyseRuler
    {
        public bool Enable { get; set; } = true;

        public abstract bool AnalyseAsset(UnityObject uObj, ref int errorCode);
    }
}
