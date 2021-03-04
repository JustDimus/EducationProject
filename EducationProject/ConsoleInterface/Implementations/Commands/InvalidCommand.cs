using ConsoleInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class InvalidCommand : BaseCommand
    {
        public InvalidCommand(string commandName)
            : base(commandName)
        {
        }

        public override void Run(int accountId)
        {
            Console.WriteLine("Invalid command");
        }
    }
}
