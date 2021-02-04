namespace DotEngine.Framework.Notify
{
    public class Notifier : INotifier
    {
        protected IFacade FacadeInstance => Facade.GetInstance();

        public void SendNotification(string name, object body)
        {
            FacadeInstance.SendNotification(name, body);
        }
    }
}
