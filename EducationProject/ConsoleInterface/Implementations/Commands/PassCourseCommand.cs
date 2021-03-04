﻿using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class PassCourseCommand : BaseCommand
    {
        private IAccountService accountService;

        private ChangeAccountCourseValidator changeAccountCourseValidator;

        public PassCourseCommand(IAccountService accountService,
            ChangeAccountCourseValidator changeAccountCourseValidator,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;

            this.changeAccountCourseValidator = changeAccountCourseValidator;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Passing the course");

            Console.Write("Course ID: ");

            if (!Int32.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var changeAccountCourse = new ChangeAccountCourseDTO()
            {
                AccountId = accountId,
                Status = EducationProject.Core.Models.Enums.ProgressStatus.Passed,
                CourseId = courseId
            };

            if(!this.ValidateEntity(changeAccountCourse))
            {
                return;
            }

            var actionResult = await accountService.ChangeAccountCourseStatusAsync(changeAccountCourse);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }

        private bool ValidateEntity(ChangeAccountCourseDTO changeAccountCourse)
        {
            var validationresult = this.changeAccountCourseValidator.Validate(changeAccountCourse);

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
