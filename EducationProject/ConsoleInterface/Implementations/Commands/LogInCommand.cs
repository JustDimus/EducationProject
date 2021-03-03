using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class LogInCommand : BaseCommand
    {
        private IAccountService accountService;

        public LogInCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Logginning");

            Console.Write("Email: ");

            var email = Console.ReadLine();

            Console.Write("Password: ");

            var password = Console.ReadLine();

            string newToken = accountService.LogIn(new AccountAuthorizationDataDTO()
            {
                Email = email,
                Password = password
            });

            if(newToken == null)
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
