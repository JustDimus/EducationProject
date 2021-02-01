using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class AddExistingMaterialToCourseChain : IChain
    {
        public string Name => "AddExistingMaterialToCourse";

        ICommandHandler _commands;

        public AddExistingMaterialToCourseChain(ICommandHandler commands)
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
                    Result = $"Invalid data count: AddExistingMaterialToCourseChain"
                };
            }

            var instResult = _commands["AuthenticateAccount"].Handle(Params);

            if (instResult.Status == ResultType.Failed)
            {
                return instResult;
            }

            instResult = _commands["IsCourseExist"].Handle(new object[] { Params[1] });

            if(instResult.Status == ResultType.Failed)
            {
                return instResult;
            }

            instResult = _commands["IsMaterialExist"].Handle(new object[] { Params[2] });

            if (instResult.Status == ResultType.Failed)
            {
                return instResult;
            }

            return _commands["AddExistingMaterialToCourse"].Handle(Params);
        }
    }
}
