﻿using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetSkillsCommand : BaseCommand
    {
        private ISkillService skillService;

        private int pageSize;

        private PageInfoValidator pageInfoValidator;

        public GetSkillsCommand(ISkillService skillService,
            PageInfoValidator pageInfoValidator,
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.skillService = skillService;

            this.pageSize = defaultPageSize;

            this.pageInfoValidator = pageInfoValidator;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Getting skills");

            Console.Write("Enter the page: ");

            if(!Int32.TryParse(Console.ReadLine(), out int pageNumber))
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var pageInfo = new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            if(!this.ValidateEntity(pageInfo))
            {
                return;
            }

            var skillsData = await skillService.GetAsync(pageInfo);

            if (!skillsData.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendJoin("\n",
                skillsData.Result.Select(s => $"{s.Id}: {s.Title}. Max value: {s.MaxValue}\n"));

            Console.WriteLine(builder);
        }

        private bool ValidateEntity(PageInfoDTO pageInfo)
        {
            var validationresult = this.pageInfoValidator.Validate(pageInfo);

            if (!validationresult.IsValid)
            {
                Console.WriteLine(String.Join("\n", validationresult.Errors));
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}
