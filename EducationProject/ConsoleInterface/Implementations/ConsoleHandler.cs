using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.Infrastructure.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations
{
    public class ConsoleHandler : IConsoleHandler
    {
        private ICommandHandler commands;

        private string exitCommand;

        private string logInCommand;

        private string logOutCommand;

        private string accountToken;

        private AuthorizationService authorizationService;

        public ConsoleHandler(
            ICommandHandler commandList,
            AuthorizationService authorizationService,
            string logInCommand,
            string logOutCommand,
            string exitCommand)
        {
            this.authorizationService = authorizationService;

            this.exitCommand = exitCommand;

            this.commands = commandList;

            this.logInCommand = logInCommand;

            this.logOutCommand = logOutCommand;
        }

        public void Run()
        {
            while (true)
            {
                var currentCommand = Console.ReadLine();

                if (currentCommand == this.exitCommand)
                {
                    break;
                }

                this.OperateCommand(currentCommand);
            }
        }

        private void OperateCommand(string command)
        {
            if (command == this.logInCommand)
            {
                this.LogIn();
                return;
            }

            if (command == this.logOutCommand)
            {
                this.LogOut();
            }

            int accountId = this.authorizationService.AuthenticateAccountAsync(this.accountToken).Result;

            this.commands[command].Run(accountId);
        }

        private async void LogIn()
        {
            Console.WriteLine("Logginning");

            Console.Write("Email: ");

            var email = Console.ReadLine();

            Console.Write("Password: ");

            var password = Console.ReadLine();

            var newToken = await this.authorizationService.AuthorizeAccountAsync(email, password);

            if (newToken == null)
            {
                Console.WriteLine("Error");
            }
            else
            {
                this.accountToken = newToken;

                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }

        private async void LogOut()
        {
            Console.WriteLine("Trying to logout");

            var logOutResult = await this.authorizationService.DeauthorizeAccountAsync(this.accountToken);

            if (logOutResult)
            {
                Console.WriteLine("Successful");
                this.accountToken = null;
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine();
        }
    }
}
