using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
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

        private IMapping<BaseMaterialDBO> materials;

        public IsMaterialExistCommand(IMapping<BaseMaterialDBO> materialMapping)
        {
            materials = materialMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            int? materialId = Params[0] as int?;

            if (materialId is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid material data: IsMaterialExistCommand"
                };
            }

            if(materials.Any(m => m.Id == materialId) == false)
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
