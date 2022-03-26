namespace DotEngine.Config.Ini
{
    public interface IDeepCopy<T> where T:class
    {
        T DeepCopy();
    }
}
