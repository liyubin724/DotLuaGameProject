namespace DotEditor.AssetChecker
{
    [MatchFilter("Match Filter","And")]
    public class AndMatchFilter : ComposeMatchFilter
    {
        public AndMatchFilter()
        {
        }

        public AndMatchFilter(params IMatchFilter[] datas) : base(datas)
        {
        }

        public override bool IsMatch(string assetPath)
        {
            foreach(var filter in filters)
            {
                if(!filter.IsMatch(assetPath))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
