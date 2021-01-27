using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class IsMaterialExistCommand : ICommand
    {
        public string Name => "IsMaterialExist";

        private IMapping<BaseMaterial> _materials;

        public IsMaterialExistCommand(IMapping<BaseMaterial> materials)
        {
            _materials = materials;
        }

        public IOperationResult Handle(object[] Params)
        {
            int materialId = 0;

            Convert.ToInt32(Params[0]);

            if (materialId == 0)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid material data: IsMaterialExistCommand"
                };
            }

            if(_materials.Get(materialId) == null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such material doesn't exist: IsMaterialExistCommand"
                };
            }
            else
            {
                return new OperationResult()
                {
                    Status = ResultType.Success,
                    Result = $"Material exists: IsMaterialExistCommand"
                };
            }
        }
    }
}
