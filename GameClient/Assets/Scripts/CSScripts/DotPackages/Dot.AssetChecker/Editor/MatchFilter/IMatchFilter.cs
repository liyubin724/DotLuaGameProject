namespace DotEditor.AssetChecker
{
    public interface IMatchFilter
    {
        bool IsMatch(string assetPath);
    }
}