namespace DotEngine.Framework.Services
{
    public class UpdateService : IUpdateService
    {
        public readonly static string NAME = "UpdateService";

        public string Name { get; private set; }
        public bool Enable { get; set; }
        public event UpdateHandler Handler;

        public UpdateService(string name)
        {
            Name = name;
        }

        public void DoRegistered()
        {
        }

        public void DoUnregistered()
        {
         
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if(Enable)
            {
                Handler?.Invoke(deltaTime, unscaleDeltaTime);
            }
        }
    }
}
