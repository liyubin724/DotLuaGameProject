namespace DotEngine.Framework.Services
{
    public class LateUpdateService : Service, ILateUpdateService
    {
        public readonly static string NAME = "LateUpdateService";

        public event LateUpdateHandler Handler;

        public LateUpdateService():base(NAME)
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
