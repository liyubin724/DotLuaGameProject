using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;

namespace DotEngine.Framework.Dispatcher
{
    public interface IServiceDispatcher : IDispatcher<string, IService>, IUpdate, ILateUpdate, IFixedUpdate
    {
    }
}
