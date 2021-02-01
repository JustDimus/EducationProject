using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class CreateMaterialChain : IChain
    {
        public string Name => "CreateMaterial";


        ICommandHandler _commands;

        public CreateMaterialChain(ICommandHandler Commands)
        {
            _commands = Commands;
        }

        public IOperationResult Handle(object[] Params)//AccountData/Title/Desc/Type/...Data...
        {
            if(Params.Length < 6)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: CreateMaterialChain"
                };
            }

            var authenticationResult = _commands["AuthenticateAccount"].Handle(Params);

            if (authenticationResult.Status == ResultType.Failed)
            {
                return authenticationResult;
            }

            return _commands["CreateMaterial"].Handle(Params);
        }
    }
}
