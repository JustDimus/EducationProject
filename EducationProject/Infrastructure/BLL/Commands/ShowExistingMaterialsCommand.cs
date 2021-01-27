using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class ShowExistingMaterialsCommand: ICommand
    {
        public string Name => "ShowExistingSkills";

        private IMapping<BaseMaterial> _materials;

        public ShowExistingMaterialsCommand(IMapping<BaseMaterial> materials)
        {
            _materials = materials;
        }

        public IOperationResult Handle(object[] Params)
        {
            Predicate<BaseMaterial> condition = Params[0] as Predicate<BaseMaterial>;

            var materialsData = _materials.Get(t => true);

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = condition is null ? materialsData : materialsData.Where(c => condition(c) == true)
            };
        }
    }
}
