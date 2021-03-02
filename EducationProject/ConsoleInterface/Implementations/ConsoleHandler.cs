using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations
{
    
    public class ConsoleHandler: IConsoleHandler
    {
        private ICommandHandler commands;

        private string token = null;

        public ConsoleHandler(ICommandHandler commandList)
        {
            commands = commandList;
        }

        public void Run()
        {
            string currentCommand = null;

            while(true)
            {
                currentCommand = Console.ReadLine();

                if(currentCommand == ConsoleCommands.ExitCommand)
                {
                    break;
                }

                commands[currentCommand].Run(ref token);
            }
        }
    }
    
}
