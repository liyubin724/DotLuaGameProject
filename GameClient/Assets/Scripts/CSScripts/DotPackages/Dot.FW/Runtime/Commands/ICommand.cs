using DotEngine.Framework.Notify;
using SystemObject = System.Object;

namespace DotEngine.Framework.Commands
{
    public interface ICommand : INotifier
    {
        void Execute(string name, SystemObject data);
    }
}
