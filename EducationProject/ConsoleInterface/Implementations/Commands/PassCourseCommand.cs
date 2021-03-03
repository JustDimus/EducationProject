using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class PassCourseCommand : BaseCommand
    {
        private IAccountService accountService;

        public PassCourseCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Passing the course");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out int courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = accountService.ChangeAccountCourseStatus(new ChangeAccountCourseDTO()
            {
                Token = token,
                Status = EducationProject.Core.Models.Enums.ProgressStatus.Passed,
                CourseId = courseId
            });

            if (actionResult == false)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }
    }
}
