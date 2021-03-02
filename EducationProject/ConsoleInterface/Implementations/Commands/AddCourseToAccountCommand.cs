using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddCourseToAccountCommand : ICommand
    {
        public string Name => "_addCourseToAccount";

        private IAccountService accounts;

        public AddCourseToAccountCommand(IAccountService accountService)
        {
            this.accounts = accountService;
        }

        public void Run(ref string token)
        {
            int courseId = 0;

            Console.WriteLine("Adding course to account");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if(this.accounts.AddAccountCourse(new ChangeAccountCourseDTO()
            {
                CourseId = courseId,
                Token = token
            }) == false)
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
