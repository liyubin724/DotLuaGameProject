using DotEngine.Framework.Dispatcher;
using Game.Services;

namespace Game.Dispatchers
{
    public class GameServiceDispatcher : ServiceDispatcher
    {
        protected override void DoInitalized()
        {
            base.DoInitalized();
            Register(TimerService.NAME, new TimerService());
            Register(GameObjectPoolService.NAME, new GameObjectPoolService());
        }

        protected override void DoDisposed()
        {
            Unregister(TimerService.NAME);
            base.DoDisposed();
        }
    }
}
