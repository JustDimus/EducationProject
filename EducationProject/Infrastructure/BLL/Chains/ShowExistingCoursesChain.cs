using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class ShowExistingCoursesChain: IChain
    {
        public string Name => "ShowExistingCourses";

        private ICommandHandler _commands;

        public ShowExistingCoursesChain(ICommandHandler commands)
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
                    Result = $"Invalid data count: ShowExistingCoursesChain"
                };
            }

            return _commands["ShowExistingCourses"].Handle(Params);
        }
    }
}
