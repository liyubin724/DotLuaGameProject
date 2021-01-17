namespace DotEditor.AssetChecker
{
    public class Matcher
    {
        public bool enable = true;
        public ComposeMatchFilter Filter { get; set; } = null;

        public bool IsMatch(string assetPath)
        {
            if(!enable || Filter == null)
            {
                return false;
            }

            return Filter.IsMatch(assetPath);
        }
    }
}
