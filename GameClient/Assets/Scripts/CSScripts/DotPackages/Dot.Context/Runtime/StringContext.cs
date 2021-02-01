namespace DotEngine.Context
{
    public class StringContext : EnvContext<string>
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
