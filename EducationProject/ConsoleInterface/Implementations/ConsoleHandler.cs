using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using Infrastructure.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations
{
    public class ConsoleHandler: IConsoleHandler
    {
        private ICommandHandler commands;

        private string exitCommand;

        private string logInCommand;

        private string logOutCommand;

        private string accountToken;

        private AuthorizationService authrizationService;

        public ConsoleHandler(ICommandHandler commandList,
            AuthorizationService authrizationService,
            string logInCommand,
            string logOutCommand,
            string exitCommand)
        {
            this.authrizationService = authrizationService;

            this.exitCommand = exitCommand;

            this.commands = commandList;

            this.logInCommand = logInCommand;

            this.logOutCommand = logOutCommand;
        }

        public void Run()
        {
            while(true)
            {
                var currentCommand = Console.ReadLine();

                if(currentCommand == this.exitCommand)
                {
                    break;
                }

                OperateCommand(currentCommand);
            }
        }

        private async void OperateCommand(string command)
        {
            if(command == this.logInCommand)
            {
                this.LogIn();
                return;
            }

            if(command == this.logOutCommand)
            {
                this.LogOut();
            }

            int accountId = await this.authrizationService.AuthenticateAccountAsync(this.accountToken);

            commands[command].Run(accountId);
        }

        private async void LogIn()
        {
            Console.WriteLine("Logginning");

            Console.Write("Email: ");

            var email = Console.ReadLine();

            Console.Write("Password: ");

            var password = Console.ReadLine();

            var newToken = await this.authrizationService.AuthorizeAccountAsync(email, password);

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

            var logOutResult = await this.authrizationService.DeauthorizeAccountAsync(this.accountToken);

            if(logOutResult)
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
