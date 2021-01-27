using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class IsAccountExistCommand: ICommand
    {
        public string Name => "IsAccountExist";

        private IMapping<SkillBO> _accounts;

        public IsAccountExistCommand(IMapping<SkillBO> accounts)
        {
            _accounts = accounts;
        }

        public IOperationResult Handle(object[] Params)
        {
            int? accountId = Params[0] as int?;

            if (accountId is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid account data: IsAccountExistCommand"
                };
            }

            if (_accounts.Get(accountId.GetValueOrDefault()) == null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such account doesn't exist: IsAccountExistCommand"
                };
            }
            else
            {
                return new OperationResult()
                {
                    Status = ResultType.Success,
                    Result = $"Account exists: IsAccountExistCommand"
                };
            }
        }
    }
}
