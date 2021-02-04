namespace DotEngine.Framework.Services
{
    public class FixedUpdateService : IFixedUpdateService
    {
        public readonly static string NAME = "FixedUpdateService";

        public string Name { get; private set; }
        public bool Enable { get; set; } = true;
        public event FixedUpdateHandler Handler;

        public FixedUpdateService(string name)
        {
            Name = name;
        }

        public void DoRegistered()
        {
        }

        public void DoUnregistered()
        {
        }

        public void DoFixedUpdate(float deltaTime)
        {
            if (Enable)
            {
                Handler?.Invoke(deltaTime);
            }
        }

    }
}
