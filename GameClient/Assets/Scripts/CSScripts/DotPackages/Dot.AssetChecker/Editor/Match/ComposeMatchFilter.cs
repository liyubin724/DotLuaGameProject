using System.Collections.Generic;

namespace DotEditor.AssetChecker
{
    public abstract class ComposeMatchFilter : MatchFilter
    {
        protected List<IMatchFilter> filters = null;

        public void Add(IMatchFilter filter)
        {
            if(filters == null)
            {
                filters = new List<IMatchFilter>();
            }
            filters.Add(filter);
        }

        public void Remove(IMatchFilter filter)
        {
            filters?.Remove(filter);
        }

        public void Clear()
        {
            filters?.Clear();
        }

        protected override void CloneTo(MatchFilter filter)
        {
            ComposeMatchFilter cmf = filter as ComposeMatchFilter;
            if(filters!=null && filters.Count>0)
            {
                for(int i =0;i<filters.Count;++i)
                {
                    if(filters[i]!=null)
                    {
                        cmf.filters.Add((IMatchFilter)filters[i].Clone());
                    }
                }
            }
        }
    }

    public class AndMatchFilter : ComposeMatchFilter
    {
        protected override bool MatchAsset(string assetPath)
        {
            foreach (var filter in filters)
            {
                if (!filter.IsMatch(assetPath))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class OrMatchFilter : ComposeMatchFilter
    {
        protected override bool MatchAsset(string assetPath)
        {
            foreach (var filter in filters)
            {
                if (filter.IsMatch(assetPath))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
