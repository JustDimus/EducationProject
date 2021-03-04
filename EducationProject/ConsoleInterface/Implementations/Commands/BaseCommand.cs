using ConsoleInterface.Interfaces;
using Infrastructure.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInterface.Implementations.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public string Name => this.name;

        protected string name;

        public BaseCommand(string commandName)
        {
            this.name = commandName;
        }

        public abstract void Run(int accountId);
    }
}
