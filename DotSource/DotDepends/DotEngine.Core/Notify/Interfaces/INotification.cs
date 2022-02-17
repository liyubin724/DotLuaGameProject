namespace DotEngine.Notify
{
    public interface INotification
    {
        string Name { get; }
        object Body { get; set; }
    }
}
