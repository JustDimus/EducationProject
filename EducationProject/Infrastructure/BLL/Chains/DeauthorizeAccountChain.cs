using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class DeauthorizeAccountChain : IChain
    {
        public string Name => "DeauthorizeAccount";

        private ICommandHandler _commands;

        public DeauthorizeAccountChain(ICommandHandler Commands)
        {
            _commands = Commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if(Params.Length < 1)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: DeauthorizeAccountChain"
                };
            }

            return _commands["DeauthorizeAccount"].Handle(Params);
        }
    }
}
