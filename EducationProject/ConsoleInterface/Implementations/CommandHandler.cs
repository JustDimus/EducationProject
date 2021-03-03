﻿using ConsoleInterface.Interfaces;
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

        public CommandHandler(IEnumerable<ICommand> commands,
            string helpCommand,
            string invalidCommand)
        {
            this.helpCommand = helpCommand;

            this.invalidCommand = invalidCommand;

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
                if(value == helpCommand)
                {
                    Console.WriteLine(String.Join("\n", commandList.Keys));
                }

                if(commandList.ContainsKey(value) == true)
                {
                    return commandList[value];
                }
                else
                {
                    return commandList[invalidCommand];
                }
            }
        }
    }
}
