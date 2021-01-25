using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Realizations
{
    public class ConsoleHandler: IConsoleHandler
    {
        private IChainHandler _commands;

        public ConsoleHandler(IChainHandler Commands)
        {
            _commands = Commands;
        }
        public void Run()
        {
            string currentMessage = "";

            //Console.WriteLine(_commands["CreateAccount"].Handle(new object[] { "User1", "Password1" }).Result.ToString());

            AccountAuthenticationData accountData = null;

            var result = _commands["AuthorizeAccount"]
                .Handle(new object[] { "User1", "Password1" });

            if(result.Status == ResultType.Error)
            {
                Console.WriteLine(result.Result);
            }
            else
            {
                accountData = (AccountAuthenticationData)_commands["AuthorizeAccount"]
                .Handle(new object[] { "User1", "Password1" }).Result;

                Console.WriteLine($"Account {accountData.Login}\n{accountData.AccountData.PhoneNumber}");
            }

            if(accountData != null)
            {
//                Console.WriteLine(_commands["CreateCourse"].Handle(new object[] { accountData, "Suck a dick", "You will know how to suck carefully" }).Result);
            }
        }
    }
}
