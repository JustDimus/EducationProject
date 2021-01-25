using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class CreateCourseChain : IChain
    {
        public string Name => "CreateCourse";

        ICommandHandler _commands;

        public CreateCourseChain(ICommandHandler Commands)
        {
            _commands = Commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if(Params.Length < 3)
            {
                return new OperationResult()
                {
                    Status = ResultType.Error,
                    Result = $"Invalid data count: CreateCourseChain"
                };
            }

            var authenticationResult = _commands["AuthenticateAccount"].Handle(Params);

            if(authenticationResult.Status == ResultType.Error)
            {
                return authenticationResult;
            }

            return _commands["CreateCourse"].Handle(Params);
        }
    }
}
