using EducationProject.BLL.Interfaces;
using EducationProject.Core.PL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Chains
{
    public class ShowExistingMaterialsChain: IChain
    {
        public string Name => "ShowExistingMaterials";

        private ICommandHandler _commands;

        public ShowExistingMaterialsChain(ICommandHandler commands)
        {
            _commands = commands;
        }

        public IOperationResult Handle(object[] Params)
        {
            if (Params.Length < 1)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data count: ShowExistingMaterialsChain"
                };
            }

            return _commands["ShowExistingMaterials"].Handle(Params);
        }
    }
}
