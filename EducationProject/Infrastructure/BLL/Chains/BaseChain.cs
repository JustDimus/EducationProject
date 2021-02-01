using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public abstract class BaseChain : IChain
    {
        public abstract string Name { get; }

        public abstract int MinParamsCount { get; }

        public virtual IOperationResult Handle(object[] Params)
        {
            if(Params.Length < MinParamsCount)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: {Name}Chain\nRequired: {MinParamsCount}, Current: {Params.Length}"
                };
            }
            else
            {
                return new OperationResult()
                {
                    Status = ResultType.Success
                };
            }
        }
    }
}
