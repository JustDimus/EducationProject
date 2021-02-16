using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class ShowExistingSkillsChain: IChain
    {
        public string Name => "ShowExistingSkills";

        private ICommandHandler _commands;

        public ShowExistingSkillsChain(ICommandHandler commands)
        {
            _commands = commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if (Params.Length < 2)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: ShowExistingSkillsChain"
                };
            }

            return _commands["ShowExistingSkills"].Handle(Params);
        }
    }
}
