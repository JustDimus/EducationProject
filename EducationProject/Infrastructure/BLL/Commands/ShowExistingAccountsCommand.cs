using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.Core.PL;
using EducationProject.Core.PL.EF;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class ShowExistingAccountsCommand : ICommand
    {
        public string Name => "ShowExistingAccounts";

        private IMapping<AccountBO> _accounts;

        private AccountConverterService _converter;

        public ShowExistingAccountsCommand(IMapping<AccountBO> accounts, AccountConverterService converter)
        {
            _accounts = accounts;

            _converter = converter;
        }

        public IOperationResult Handle(object[] Params)
        {
            Expression<Func<AccountDBO, bool>> condition = Params[0] as Expression<Func<AccountDBO, bool>>;

            int? startPage = Params[1] as int?;

            int? pageSize = 30;

            if(Params.Length > 2)
            {
                pageSize = Params[2] as int?;
            }

            var accountData = new List<AccountPL>();
               // _converter.ConvertBLLToPL(_accounts.Get(t => true));

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = null//TODO
            };
        }
    }
}
