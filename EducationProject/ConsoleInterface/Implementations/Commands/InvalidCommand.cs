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

        public override void Run(ref string token)
        {
            Console.WriteLine("Invalid command");
        }
    }
}
