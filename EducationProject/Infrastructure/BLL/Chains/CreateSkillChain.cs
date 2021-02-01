using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class CreateSkillChain : IChain
    {
        public string Name => "CreateSkill";

        ICommandHandler _commands;

        public CreateSkillChain(ICommandHandler commands)
        {
            _commands = commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if(Params.Length < 3)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: CreateSkillChain"
                };
            }

            var authResult = _commands["AuthenticateAccount"].Handle(Params);

            if(authResult.Status == ResultType.Failed)
            {
                return authResult;
            }

            return _commands["CreateSkill"].Handle(Params);
        }
    }
}
