using DotEngine.Framework.Updater;

namespace DotEngine.Framework.Services
{
    public delegate void UpdateHandler(float deltaTime, float unscaleDeltaTime);

    public interface IUpdateService : IUpdate, IService
    {
        event UpdateHandler Handler;
    }
}
