using ConsoleInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations
{
    public class CommandHandler : ICommandHandler
    {
        private Dictionary<string, ICommand> commandList;

        public CommandHandler(IEnumerable<ICommand> commands)
        {
            commandList = new Dictionary<string, ICommand>();

            foreach(var command in commands)
            {
                commandList.Add(command.Name, command);
            }
        }

        public ICommand this[string value]
        {
            get
            {
                if(commandList.ContainsKey(value) == true)
                {
                    return commandList[value];
                }
                else
                {
                    return commandList["InvalidCommand"];
                }
            }
        }
    }
}
