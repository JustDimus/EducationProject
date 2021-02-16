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
    public class CreateSkillCommand : ICommand
    {
        public string Name => "CreateSkill";

        private IMapping<SkillDBO> skills;

        public CreateSkillCommand(IMapping<SkillDBO> skillMapping)
        {
            skills = skillMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            string title = Params[1] as string;

            int? maxValue = Params[2] as int?;
            
            if(maxValue.HasValue == false || String.IsNullOrEmpty(title))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid skill data: CreateSkillCommand"
                };
            }

            SkillDBO skill = new SkillDBO()
            {
                MaxValue = maxValue.Value,
                Title = title
            };

            skills.Create(skill);

            skills.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created skill by account {account.AccountId} with name {skill.Title}. Id: {skill.Id}"
            };
        }
    }
}
