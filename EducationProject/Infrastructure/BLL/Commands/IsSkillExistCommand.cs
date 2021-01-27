using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
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

        private IMapping<SkillBO> _skills;

        public IsSkillExistCommand(IMapping<SkillBO> skills)
        {
            _skills = skills;
        }

        public IOperationResult Handle(object[] Params)
        {
            int skillId = 0;

            Convert.ToInt32(Params[0]);

            if (skillId == 0)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid skill data: IsSkillExistCommand"
                };
            }

            if (_skills.Get(skillId) == null)
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
