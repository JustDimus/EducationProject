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
    public class IsSkillExistCommand: ICommand
    {
        public string Name => "IsSkillExist";

        private IMapping<SkillDBO> skills;

        public IsSkillExistCommand(IMapping<SkillDBO> skillMapping)
        {
            skills = skillMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            int? skillId = Params[0] as int?;

            if (skillId is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid skill data: IsSkillExistCommand"
                };
            }

            if (skills.Any(s => s.Id == skillId) == false)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Such skill doesn't exist: IsSkillExistCommand"
                };
            }
            else
            {
                return new OperationResult()
                {
                    Status = ResultType.Success,
                    Result = $"Skill exists: IsSkillExistCommand"
                };
            }
        }
    }
}
