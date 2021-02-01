using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class MoveCourseToPassedInAccountChain : IChain
    {
        public string Name => "MoveCourseToPassed";

        private ICommandHandler _commands;

        public MoveCourseToPassedInAccountChain(ICommandHandler commands)
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
                    Result = $"Invalid data count: MoveCourseToPassedChain"
                };
            }

            var instResult = _commands["IsAccountExist"].Handle(new object[] { Params[1] });

            if(instResult.Status == ResultType.Failed)
            {
                return instResult;
            }

            instResult = _commands["IsCourseExist"].Handle(new object[] { Params[2] });

            if(instResult.Status == ResultType.Failed)
            {
                return instResult;
            }

            return _commands["MoveCourseToPassed"].Handle(Params);
        }
    }
}
