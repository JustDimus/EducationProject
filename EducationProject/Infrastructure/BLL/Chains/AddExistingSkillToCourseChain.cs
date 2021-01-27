using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class AddExistingSkillToCourseChain: IChain
    {
        public string Name => "AddExistingSkillToCourse";

        ICommandHandler _commands;

        public AddExistingSkillToCourseChain(ICommandHandler commands)
        {
            _commands = commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if (Params.Length < 4)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: AddExistingSkillToCourseChain"
                };
            }

            var instResult = _commands["AuthenticateAccount"].Handle(Params);

            if (instResult.Status == ResultType.Failed)
            {
                return instResult;
            }

            instResult = _commands["IsCourseExist"].Handle(new object[] { Params[1] });

            if (instResult.Status == ResultType.Failed)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such course doesn't exist: AddExistingMaterialToCourseChain"
                };
            }

            instResult = _commands["IsSkillExist"].Handle(new object[] { Params[2] });

            if (instResult.Status == ResultType.Failed)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such material doesn't exist: AddExistingMaterialToCourseChain"
                };
            }

            return _commands["AddExistingSkillToCourse"].Handle(Params);
        }
    }
}
