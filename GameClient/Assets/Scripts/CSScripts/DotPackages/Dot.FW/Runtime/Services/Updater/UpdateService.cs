namespace DotEngine.Framework.Services
{
    public class UpdateService : Service,IUpdateService
    {
        public readonly static string NAME = "UpdateService";

        public event UpdateHandler Handler;

        public UpdateService():base(NAME)
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
