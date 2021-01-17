using System.Collections.Generic;

namespace DotEditor.AssetChecker
{
    public abstract class ComposeMatchFilter : IMatchFilter
    {
        protected List<IMatchFilter> filters = new List<IMatchFilter>();

        protected ComposeMatchFilter()
        {

        }

        protected ComposeMatchFilter(params IMatchFilter[] datas)
        {
            if(datas != null)
            {
                for(int i =0;i<datas.Length;++i)
                {
                    filters.Add(datas[i]);
                }
            }
        }

        public void Add(IMatchFilter filter)
        {
            filters.Add(filter);
        }

        public void Remove(IMatchFilter filter)
        {
            filters.Remove(filter);
        }

        public void Clear()
        {
            filters.Clear();
        }

        public abstract bool IsMatch(string assetPath);
    }
}
