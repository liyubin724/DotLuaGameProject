namespace DotEngine.Config
{
    internal interface IDeepCopy<T> where T:class
    {
        T DeepCopy();
    }
}
