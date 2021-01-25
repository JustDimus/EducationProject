using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Infrastructure.BLL
{
    public class CommandHandler : ICommandHandler
    {
        public Dictionary<string, ICommand> _commands;

        public CommandHandler(IEnumerable<ICommand> Commands)
        {
            _commands = new Dictionary<string, ICommand>();

            foreach (var command in Commands)
            {
                _commands.Add(command.Name, command);
            }
        }

        public ICommand this[string Command] 
        { 
            get
            {
                if(_commands.ContainsKey(Command))
                {
                    return _commands[Command];
                }
                else
                {
                    return _commands["Error"];
                }
            }
        }
    }
}
