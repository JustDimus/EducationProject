using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
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

        private IMapping<SkillBO> _skills;

        public CreateSkillCommand(IMapping<SkillBO> skills)
        {
            _skills = skills;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            string title = Params[1] as string;

            int maxValue = Convert.ToInt32(Params[2]);
            
            if(maxValue == 0 || String.IsNullOrEmpty(title))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid skill data: CreateSkillCommand"
                };
            }

            SkillBO skill = new SkillBO()
            {
                MaxValue = maxValue,
                Title = title
            };

            _skills.Create(skill);

            _skills.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created skill by account {account.AccountData.Id} with name {skill.Title}. Id: {skill.Id}"
            };
        }
    }
}
