using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class CreateAccountChain : IChain
    {
        public string Name => "CreateAccount";

        ICommandHandler _commands;

        public CreateAccountChain(ICommandHandler Commands)
        {
            _commands = Commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if(Params.Length < 2)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "Invalid data count: CreateAccountChain"
                };
            }

            return _commands["CreateAccount"].Handle(Params);
        }
    }
}
