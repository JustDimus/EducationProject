using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class ShowExistingAccountsChain : IChain
    {
        public string Name => "ShowExistingAccounts";

        private ICommandHandler _commands;

        public ShowExistingAccountsChain(ICommandHandler commands)
        {
            _commands = commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if(Params.Length < 1)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: ShowExistingAccountsChain"
                };
            }

            return _commands["ShowExistingAccounts"].Handle(Params);
        }
    }
}
