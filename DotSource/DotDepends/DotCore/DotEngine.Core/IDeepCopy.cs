namespace DotEngine
{
    public interface IDeepCopy
    {
        object DeepCopy();
    }

    public interface IDeepCopy<T> where T : class
    {
        T DeepCopy();
    }
}
