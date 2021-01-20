namespace DotEditor.GUIExt
{
    public interface IMatchFilter<T>
    {
        bool IsMatch(T value);
    }

    public interface IStringFilter : IMatchFilter<string>
    {

    }
}
