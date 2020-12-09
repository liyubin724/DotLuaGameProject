using System;

namespace DotEngine.Context
{
    public class TypeContext : EnvContext<Type>
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
