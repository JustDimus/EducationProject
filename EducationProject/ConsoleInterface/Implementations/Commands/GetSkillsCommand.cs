using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetSkillsCommand : BaseCommand
    {
        private ISkillService skills;

        private int pageSize;

        public GetSkillsCommand(ISkillService skillService, 
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.skills = skillService;

            this.pageSize = defaultPageSize;
        }

        public override void Run(ref string token)
        {
            int pageNumber = 0;

            Console.WriteLine("Getting skills");

            Console.Write("Enter the page: ");

            Int32.TryParse(Console.ReadLine(), out pageNumber);

            var skillsData = skills.Get(new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            StringBuilder builder = new StringBuilder();

            foreach(var skill in skillsData)
            {
                builder.Append($"{skill.Id}: {skill.Title}. Max value: {skill.MaxValue}\n");
            }

            Console.WriteLine(builder);
        }
    }
}
