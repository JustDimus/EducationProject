using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class ErrorChain : IChain
    {
        public string Name => "Error";

        private ICommandHandler _commands;

        public ErrorChain(ICommandHandler Commands)
        {
            _commands = Commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            return _commands["Error"].Handle(Params);
        }
    }
}
