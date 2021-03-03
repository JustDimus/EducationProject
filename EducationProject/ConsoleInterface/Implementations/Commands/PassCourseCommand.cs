using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class PassCourseCommand : BaseCommand
    {
        private IAccountService accounts;

        public PassCourseCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accounts = accountService;
        }

        public override void Run(ref string token)
        {
            int courseId = 0;

            Console.WriteLine("Passing the course");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if (accounts.ChangeAccountCourseStatus(new ChangeAccountCourseDTO()
            {
                Token = token,
                Status = EducationProject.Core.DAL.EF.Enums.ProgressStatus.Passed,
                CourseId = courseId
            }) == true)
            {
                Console.WriteLine("Successful");
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine();
        }
    }
}
