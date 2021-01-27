using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class AuthenticateAccountCommand : ICommand
    {
        public string Name => "AuthenticateAccount";

        private AuthorizationService _authorization;

        public AuthenticateAccountCommand(AuthorizationService Authorization)
        {
            _authorization = Authorization;
        }

        public IOperationResult Handle(object[] Params)
        {
            if(Params.Length < 1)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "Invalid data: authentication"
                };
            }

            var account = Params[0] as AccountAuthenticationData;

            if(account is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "Invalid data: authentication"
                };
            }

            return _authorization.AuthenticateAccount(account.Token);
        }
    }
}
