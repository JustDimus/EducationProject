using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateAccountCommand : ICommand
    {
        public string Name => "_createAccount";

        private IAccountService accounts;

        public CreateAccountCommand(IAccountService accountService)
        {
            accounts = accountService;
        }

        public void Run(ref string token)
        {
            string email = null;

            string password = null;
            
            Console.WriteLine("Creating new account");

            Console.Write("Email: ");

            email = Console.ReadLine();

            Console.Write("Password: ");

            password = Console.ReadLine();

            if(accounts.Create(new ChangeEntityDTO<ShortAccountInfoDTO>()
            {
                Token = token,
                Entity = new ShortAccountInfoDTO()
                {
                    Email = email,
                    Password = password
                }
            }) == true)
            {
                Console.WriteLine("Succesful");
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine();
        }
    }
}
