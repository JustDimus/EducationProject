using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class AuthorizeAccountChain : IChain
    {
        public string Name => "AuthorizeAccount";

        private ICommandHandler _commands;

        public AuthorizeAccountChain(ICommandHandler Commands)
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
                    Result = $"Invalid data count: AuthorizeAccountChain"
                };
            }

            return _commands["AuthorizeAccount"].Handle(Params);
        }
    }
}
