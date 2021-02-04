using DotEngine.Framework.Dispatcher;
using Game.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Dispatchers
{
    public class GameServiceDispatcher : ServiceDispatcher
    {
        protected override void DoInitalized()
        {
            base.DoInitalized();
            Register(TimerService.NAME, new TimerService());
        }

        protected override void DoDisposed()
        {
            base.DoDisposed();
            Unregister(TimerService.NAME);
        }
    }
}
