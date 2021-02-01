using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Predicate<AccountPL> condition = Params[0] as Predicate<AccountPL>;

            var accountData = _converter.ConvertBLLToPL(_accounts.Get(t => true));

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = condition is null ? accountData : accountData.Where(c => condition(c) == true)
            };
        }
    }
}
