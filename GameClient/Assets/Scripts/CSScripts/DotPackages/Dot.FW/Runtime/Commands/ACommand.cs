using DotEngine.Framework.Notify;

namespace DotEngine.Framework.Commands
{
    public abstract class ACommand : Notifier, ICommand
    {
        public abstract void Execute(string name, object body);
    }
}
