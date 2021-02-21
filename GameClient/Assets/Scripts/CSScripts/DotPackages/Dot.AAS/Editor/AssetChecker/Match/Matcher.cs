using System;
using System.Collections.Generic;

namespace DotEditor.AssetChecker
{
    public class Matcher : ICloneable
    {
        public bool enable = true;
        public List<IMatchFilter> filters = new List<IMatchFilter>();

        public void Add(IMatchFilter filter)
        {
            if (filters == null)
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

        public bool IsMatch(string assetPath)
        {
            if(!enable)
            {
                return false;
            }

            int enableCount = 0;
            foreach (var filter in filters)
            {
                if(filter.Enable)
                {
                    enableCount++;
                }else
                {
                    continue;
                }
                if (!filter.IsMatch(assetPath))
                {
                    return false;
                }
            }

            return enableCount>0;
        }

        public object Clone()
        {
            Matcher matcher = new Matcher();
            matcher.enable = enable;
            matcher.filters = new List<IMatchFilter>();
            if(filters!=null && filters.Count>0)
            {
                for(int i =0;i<filters.Count;++i)
                {
                    if(filters[i]!=null)
                    {
                        matcher.filters.Add(filters[i]);
                    }
                }
            }
            return matcher;
        }
    }
}
