using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Notification
{
    public abstract class AObserver : IObserver
    {
        public abstract string[] ListInterestMessage();
        public abstract void HandleMessage(string name, object body = null);
    }
}
