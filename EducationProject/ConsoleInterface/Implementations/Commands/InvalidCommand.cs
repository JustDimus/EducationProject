using ConsoleInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class InvalidCommand : ICommand
    {
        public string Name => "InvalidCommand";

        public void Run(ref string token)
        {
            Console.WriteLine("Invalid command");
        }
    }
}
