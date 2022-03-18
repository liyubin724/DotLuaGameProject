namespace DotEngine.Injection
{
    public class InjectObject : IInjectObject
    {
        public void ExtractTo<K>(IInjectContext<K> context)
        {
            context.ExtractFrom(this);
        }

        public void InjectBy<K>(IInjectContext<K> context)
        {
            context.InjectTo(this);
        }
    }
}
