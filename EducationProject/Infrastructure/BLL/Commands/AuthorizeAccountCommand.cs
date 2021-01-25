using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class AuthorizeAccountCommand : ICommand
    {
        public string Name => "AuthorizeAccount";

        private AuthorizationService _authService;

        public AuthorizeAccountCommand(AuthorizationService Authorization)
        {
            _authService = Authorization;
        }

        public IOperationResult Handle(object[] Params)
        {
            var login = Params[0] as string;

            var password = Params[1] as string;

            if(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                return new OperationResult()
                {
                    Status = ResultType.Error,
                    Result = "Invalid data: AuthorizationCommand"
                };
            }

            return _authService.AuthorizeAccount(login, password);
        }
    }
}
