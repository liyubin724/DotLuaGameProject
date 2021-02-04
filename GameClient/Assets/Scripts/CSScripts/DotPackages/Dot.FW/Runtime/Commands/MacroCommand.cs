using System.Collections.Generic;

namespace DotEngine.Framework.Commands
{
    public class MacroCommand : ACommand
    {
        private readonly IList<ICommand> subcommands;

        public MacroCommand()
        {
            subcommands = new List<ICommand>();
            InitializeMacroCommand();
        }

        public MacroCommand(params ICommand[] commands) : this()
        {
            if(commands!=null)
            {
                foreach(var command in commands)
                {
                    subcommands.Add(command);
                }
            }
        }

        protected virtual void InitializeMacroCommand()
        {
        }

        protected void AddSubCommand(ICommand command)
        {
            subcommands.Add(command);
        }

        public override void Execute(string name, object body)
        {
            for (int i = 0; i < subcommands.Count; ++i)
            {
                subcommands[i].Execute(name, body);
            }
        }
    }
}
