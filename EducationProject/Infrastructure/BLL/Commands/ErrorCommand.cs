using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class ErrorCommand : ICommand
    {
        public string Name => "Error";

        public IOperationResult Handle(object[] Params)
        {
            return new OperationResult()
            {
                Status = ResultType.Failed,
                Result = $"Invalid command: ErrorCommand"
            };
        }
    }
}
