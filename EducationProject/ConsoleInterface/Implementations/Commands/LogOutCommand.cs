using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class LogOutCommand : ICommand
    {
        public string Name => "_logout";

        private IAccountService accounts;

        public LogOutCommand(IAccountService accountService)
        {
            accounts = accountService;
        }

        public void Run(ref string token)
        {
            Console.WriteLine("Trying to logout");

            if(accounts.LogOut(token) == true)
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
