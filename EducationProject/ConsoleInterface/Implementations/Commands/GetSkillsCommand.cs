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
        private ISkillService skillService;

        private int pageSize;

        public GetSkillsCommand(ISkillService skillService, 
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.skillService = skillService;

            this.pageSize = defaultPageSize;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Getting skills");

            Console.Write("Enter the page: ");

            if(Int32.TryParse(Console.ReadLine(), out int pageNumber) == false)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var skillsData = skillService.Get(new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            if (skillsData == null)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            StringBuilder builder = new StringBuilder();

            foreach(var skill in skillsData)
            {
                builder.Append($"{skill.Id}: {skill.Title}. Max value: {skill.MaxValue}\n");
            }

            Console.WriteLine(builder);
        }
    }
}
