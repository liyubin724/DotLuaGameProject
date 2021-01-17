namespace DotEditor.AssetChecker
{
    public abstract class MatchFilter : IMatchFilter
    {
        public abstract bool IsMatch(string assetPath);
    }
}
