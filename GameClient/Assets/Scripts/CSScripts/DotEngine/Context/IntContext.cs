namespace DotEngine.Context
{
    public class IntContext : EnvContext<int>
    {
        public void Inject(object injectObj)
        {
            ContextUtil.Inject<int>(this, injectObj);
        }

        public void Extract(object extractObj)
        {
            ContextUtil.Extract<int>(this, extractObj);
        }
    }
}
