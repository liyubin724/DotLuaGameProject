using DotEngine.Framework.Commands;
using SystemObject = System.Object;

namespace DotEngine.Framework.Dispatcher
{
    public class CommandDispatcher : ADispatcher<string, CommandCreater>, ICommandDispatcher
    {
        public void Execute(string name,SystemObject data)
        {
            CommandCreater creater = Retrieve(name);
            if (creater != null)
            {
                ICommand command = creater();
                command.Execute(name, data);
            }
        }
    }
}
