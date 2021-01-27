using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class DeauthorizeAccountCommand : ICommand
    {
        public string Name => "DeauthorizeAccount";


        private AuthorizationService _authService;

        public DeauthorizeAccountCommand(AuthorizationService authorization)
        {
            _authService = authorization;
        }

        public IOperationResult Handle(object[] Params)
        {
            string deauthToken = Params[0] as string;

            if(String.IsNullOrEmpty(deauthToken))
            {
                return new OperationResult()
                {
                    Status = ResultType.Success,
                    Result = null
                };
            }

            return _authService.DeauthorizeAccount(deauthToken);
        }
    }
}
