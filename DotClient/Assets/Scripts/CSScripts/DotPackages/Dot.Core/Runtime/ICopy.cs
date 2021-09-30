namespace DotEngine.Core
{
    public interface ICopy<T>
    {
        T Copy();
    }

    public interface IDeepCopy<T>
    {
        T DeepCopy();
    }
}
