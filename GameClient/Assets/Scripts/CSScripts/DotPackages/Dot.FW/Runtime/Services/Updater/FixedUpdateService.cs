namespace DotEngine.Framework.Services
{
    public class FixedUpdateService : Service, IFixedUpdateService
    {
        public readonly static string NAME = "FixedUpdateService";

        public event FixedUpdateHandler Handler;

        public FixedUpdateService():base(NAME)
        {
        }

        public void DoFixedUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (Enable)
            {
                Handler?.Invoke(deltaTime, unscaleDeltaTime);
            }
        }

    }
}
