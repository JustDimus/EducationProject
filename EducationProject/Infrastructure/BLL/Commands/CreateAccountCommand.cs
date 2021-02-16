using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
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

        private IMapping<AccountDBO> accounts;

        public CreateAccountCommand(IMapping<AccountDBO> accountMapping)
        {
            accounts = accountMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            string login = Params[0] as string;

            string password = Params[1] as string;

            if(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "Invalid data: AccountCommand"
                };
            }

            if(accounts.Any(a => a.Email == login))
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "Account with that login already exist!"
                };
            }

            AccountDBO account = new AccountDBO()
            {
                RegistrationDate = DateTime.Now,
                Email = login,
                Password = password
            };

            accounts.Create(account);

            accounts.Save();

            return new EducationProject.Core.PL.OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created account with login {account.Email}. Id: {account.Id}"
            };
        }
    }
}
