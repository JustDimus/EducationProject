using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class ChangeCourseStateCommand : BaseCommand
    {
        private ICourseService courseService;

        private CourseVisibilityValidator courseVisibilityValidator;

        public ChangeCourseStateCommand(
            ICourseService courseService,
            CourseVisibilityValidator courseVisibilityValidator,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.courseVisibilityValidator = courseVisibilityValidator;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Changing course state");

            Console.Write("Course ID: ");

            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("New state 0 - hide, 1 - publish: ");

            if (!int.TryParse(Console.ReadLine(), out int courseVisibilityState))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if (courseVisibilityState != 0 & courseVisibilityState != 1)
            {
                Console.WriteLine("Error. Enter the 0 or 1");
                Console.WriteLine();
                return;
            }

            var courseVisibility = new CourseVisibilityDTO()
            {
                AccountId = accountId,
                CourseId = courseId,
                Visibility = courseVisibilityState == 0 ? false : true
            };

            if (!this.ValidateEntity(courseVisibility))
            {
                return;
            }

            var actionResult = await this.courseService.ChangeCourseVisibilityAsync(courseVisibility);

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

        private bool ValidateEntity(CourseVisibilityDTO courseVisibility)
        {
            var validationresult = this.courseVisibilityValidator.Validate(courseVisibility);

            if (!validationresult.IsValid)
            {
                Console.WriteLine(string.Join("\n", validationresult.Errors));
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}
