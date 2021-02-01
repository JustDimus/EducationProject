using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL
{
    public class AuthorizationService
    {
        private List<string> _authorizedAccounts;
        
        private IMapping<AccountBO> _accounts;

        private AccountConverterService _converter;

        public AuthorizationService(IMapping<AccountBO> AccMapping, AccountConverterService converter)
        {
            _accounts = AccMapping;

            _authorizedAccounts = new List<string>();

            _converter = converter;
        }

        public IOperationResult AuthorizeAccount(string Login, string Password)
        {
            AccountBO account = _accounts.Get(a => a.Email == Login && a.Password == Password)
                .FirstOrDefault();

            if(account is null)
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "Invalid login or password"
                };
            }

            string token = $"{DateTime.Now}/{account.Email}";

            _authorizedAccounts.Add(token);

            return new EducationProject.Core.PL.OperationResult()
            {
                Status = ResultType.Success,
                Result = new EducationProject.Core.PL.AccountAuthenticationData()
                {
                    AccountData = _converter.ConvertBLLToPL(account),
                    Login = account.Email,
                    Token = token
                }
            };
        }

        public IOperationResult AuthenticateAccount(string Token)
        {
            if(_authorizedAccounts.Contains(Token))
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Success,
                    Result = true
                };
            }
            else
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "Account not authorized"
                };
            }
        }

        public IOperationResult DeauthorizeAccount(string Token)
        {
            if(_authorizedAccounts.Contains(Token))
            {
                _authorizedAccounts.Remove(Token);

                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Success,
                    Result = true
                };
            }
            else
            {
                return new EducationProject.Core.PL.OperationResult()
                {
                    Status = ResultType.Success,
                    Result = false
                };
            }
        }
    }
}
