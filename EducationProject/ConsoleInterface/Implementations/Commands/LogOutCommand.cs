using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class LogOutCommand : BaseCommand
    {
        private IAccountService accountService;

        public LogOutCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Trying to logout");

            var actionResult = accountService.LogOut(token);

            if (actionService == true)
            {
                Console.WriteLine("Successful");
                token = null;
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine();
        }
    }
}
