namespace DotEngine.Framework.Services
{
    public class LateUpdateService : ILateUpdateService
    {
        public readonly static string NAME = "LateUpdateService";

        public string Name { get; private set; }

        public bool Enable { get; set; } = true;

        public event LateUpdateHandler Handler;

        public LateUpdateService(string name)
        {
            Name = name;
        }

        public void DoRegistered()
        {
        }

        public void DoUnregistered()
        {
        }

        public void DoLateUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if(Enable)
            {
                Handler?.Invoke(deltaTime, unscaleDeltaTime);
            }
        }
    }
}
