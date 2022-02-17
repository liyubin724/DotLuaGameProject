namespace DotEngine
{
    public interface ICopy
    {
        object Copy();
    }

    public interface ICopy<T> where T : class
    {
        T Copy();
    }
}
