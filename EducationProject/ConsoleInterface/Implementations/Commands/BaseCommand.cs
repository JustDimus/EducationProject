using ConsoleInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public string Name => name;

        protected string name;

        public BaseCommand(string commandName)
        {
            this.name = commandName;
        }

        public abstract void Run(ref string token);
    }
}
