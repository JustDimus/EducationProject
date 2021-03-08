using ConsoleInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations
{
    public class CommandHandler : ICommandHandler
    {
        private Dictionary<string, ICommand> commandList;

        private string helpCommand;
        private string invalidCommand;

        public CommandHandler(
            IEnumerable<ICommand> commands,
            string helpCommand,
            string invalidCommand)
        {
            this.helpCommand = helpCommand;

            this.invalidCommand = invalidCommand;

            this.commandList = new Dictionary<string, ICommand>();

            foreach (var command in commands)
            {
                this.commandList.Add(command.Name, command);
            }
        }

        public ICommand this[string value]
        {
            get
            {
                if (value == this.helpCommand)
                {
                    Console.WriteLine(string.Join("\n", this.commandList.Keys));
                }

                if (this.commandList.ContainsKey(value))
                {
                    return this.commandList[value];
                }
                else
                {
                    return this.commandList[this.invalidCommand];
                }
            }
        }
    }
}
