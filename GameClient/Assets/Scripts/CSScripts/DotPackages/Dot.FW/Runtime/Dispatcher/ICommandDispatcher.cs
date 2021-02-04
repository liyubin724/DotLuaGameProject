using DotEngine.Framework.Commands;
using SystemObject = System.Object;

namespace DotEngine.Framework.Dispatcher
{
    public delegate ICommand CommandCreater();

    public interface ICommandDispatcher : IDispatcher<string, CommandCreater>
    {
        void Execute(string name, SystemObject data);
    }
}
