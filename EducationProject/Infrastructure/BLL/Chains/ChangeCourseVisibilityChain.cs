using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class ChangeCourseVisibilityChain : IChain
    {
        public string Name => "ChangeCourseVisibility";

        private ICommandHandler _commands;

        public ChangeCourseVisibilityChain(ICommandHandler commands)
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
                    Result = $"Invalid data count: ChangeCourseVisibilityChain"
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

            return _commands["ChangeCourseVisibility"].Handle(Params);
        }
    }
}
