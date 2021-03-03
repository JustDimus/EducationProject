using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddCourseToAccountCommand : BaseCommand
    {
        private IAccountService accountService;

        public AddCourseToAccountCommand(IAccountService accountService,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Adding course to account");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out int courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = this.accountService.AddAccountCourse(new ChangeAccountCourseDTO()
            {
                CourseId = courseId,
                Token = token
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
