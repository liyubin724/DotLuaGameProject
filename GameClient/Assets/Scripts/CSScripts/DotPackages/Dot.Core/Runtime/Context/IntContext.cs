namespace DotEngine.Context
{
    public class IntContext : EnvContext<int>
    {
        public void Inject(object injectObj)
        {
            ContextUtil.Inject(this, injectObj);
        }

        public void Extract(object extractObj)
        {
            ContextUtil.Extract(this, extractObj);
        }
    }
}
