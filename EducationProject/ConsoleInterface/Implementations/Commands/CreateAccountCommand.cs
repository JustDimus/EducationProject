using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateAccountCommand : BaseCommand
    {
        private IAccountService accountService;

        public CreateAccountCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;
        }

        public override void Run(ref string token)
        {            
            Console.WriteLine("Creating new account");

            Console.Write("Email: ");

            var email = Console.ReadLine();

            Console.Write("Password: ");

            var password = Console.ReadLine();

            if (String.IsNullOrEmpty(email) == true || String.IsNullOrEmpty(password) == true)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var actionResult = this.accountService.Create(new ChangeEntityDTO<ShortAccountInfoDTO>()
            {
                Token = token,
                Entity = new ShortAccountInfoDTO()
                {
                    Email = email,
                    Password = password
                }
            });

            if (actionResult == false)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Succesful");
            }

            Console.WriteLine();
        }
    }
}
