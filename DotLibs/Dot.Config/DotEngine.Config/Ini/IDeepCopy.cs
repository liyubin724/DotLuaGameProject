namespace DotEngine.Config.Ini
{
    internal interface IDeepCopy<T> where T : class
    {
        T DeepCopy();
    }
}
