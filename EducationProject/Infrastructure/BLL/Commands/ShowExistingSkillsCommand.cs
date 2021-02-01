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
    public class ShowExistingSkillsCommand: ICommand
    {
        public string Name => "ShowExistingSkills";

        private IMapping<SkillBO> _skills;

        public ShowExistingSkillsCommand(IMapping<SkillBO> skills)
        {
            _skills = skills;
        }

        public IOperationResult Handle(object[] Params)
        {
            Predicate<SkillBO> condition = Params[0] as Predicate<SkillBO>;

            var skillsData = _skills.Get(t => true);

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = condition is null ? skillsData : skillsData.Where(c => condition(c) == true)
            };
        }
    }
}
