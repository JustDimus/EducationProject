using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class ShowExistingSkillsCommand: ICommand
    {
        public string Name => "ShowExistingSkills";

        private IMapping<SkillDBO> skills;

        private IConverter<SkillDBO, SkillBO> converter;

        public ShowExistingSkillsCommand(IMapping<SkillDBO> skillMapping,
            IConverter<SkillDBO, SkillBO> skillConverter)
        {
            this.skills = skillMapping;

            this.converter = skillConverter;
        }

        public IOperationResult Handle(object[] Params)
        {
            Expression<Func<SkillDBO, bool>> condition = Params[0] as Expression<Func<SkillDBO, bool>>;

            if (condition is null)
            {
                condition = a => true;
            }

            int? startPage = null;

            int? pageSize = null;

            if (Params.Length > 1)
            {
                startPage = Params[1] as int?;

                pageSize = Params[2] as int?;
            }

            if (startPage.HasValue == false || pageSize.HasValue == false)
            {
                startPage = 0;

                pageSize = 30;
            }

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = converter.Get(condition, startPage.Value, pageSize.Value)
            };
        }
    }
}
