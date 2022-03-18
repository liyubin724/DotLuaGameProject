namespace DotEngine.Injection
{
    public interface IInjectObject
    {
        void InjectBy<K>(IInjectContext<K> context);
        void ExtractTo<K>(IInjectContext<K> context);
    }
}
