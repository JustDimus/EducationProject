using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class CreateAccountCommand : ICommand
    {
        public string Name => "CreateAccount";

      //  private ICommandHandler _commands;

        private IMapping<Account> _accounts;

        public CreateAccountCommand(IMapping<Account> AccMapping)
        {
           // _commands = Commands;

            _accounts = AccMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            string Login = Params[0] as string;

            string Password = Params[1] as string;

            if(String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Password))
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Error,
                    Result = "Invalid data: AccountCommand"
                };
            }

            if(_accounts.Get(t => t.Email == Login).Any() == true)
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Error,
                    Result = "Account with that login already exist!"
                };
            }

            Account account = new Account()
            {
                RegistrationDate = DateTime.Now,
                Email = Login,
                Password = Password
            };

            _accounts.Create(account);

            _accounts.Save();

            return new EducationProject.Core.PL.OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created account with login {account.Email}. Id: {account.Id}"
            };
        }
    }
}
