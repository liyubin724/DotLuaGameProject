using DotEngine.Framework.Updater;

namespace DotEngine.Framework.Services
{
    public delegate void LateUpdateHandler(float deltaTime, float unscaleDeltaTime);

    public interface ILateUpdateService : ILateUpdate, IService
    {
        event LateUpdateHandler Handler;
    }
}
