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
        private AccountSectionHandler _accountHandler;

        public ConsoleHandler(AccountSectionHandler accountHandler)
        {
            _accountHandler = accountHandler;
        }
        public void Run()
        {
            _accountHandler.Run();
        }
    }
}
