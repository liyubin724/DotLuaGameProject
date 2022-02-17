namespace DotEngine.Notify
{
    public class Notification : INotification
    {
        public string Name { get; private set; }
        public object Body { get; set; }

        public Notification(string name,object body = null)
        {
            Name = name;
            Body = body;
        }
    }
}
