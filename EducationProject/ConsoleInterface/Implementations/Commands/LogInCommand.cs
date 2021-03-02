using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class LogInCommand : ICommand
    {
        public string Name => "_login";

        private IAccountService accounts;

        public LogInCommand(IAccountService accountService)
        {
            accounts = accountService;
        }

        public void Run(ref string token)
        {
            string email = null;

            string password = null;

            Console.WriteLine("Logginning");

            Console.Write("Email: ");

            email = Console.ReadLine();

            Console.Write("Password: ");

            password = Console.ReadLine();

            string newToken = accounts.LogIn(new AccountAuthorizationDataDTO()
            {
                Email = email,
                Password = password
            });

            if(newToken is null)
            {
                Console.WriteLine("Error");
            }
            else
            {
                token = newToken;

                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }
    }
}
