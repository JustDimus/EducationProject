using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;
using System.Threading.Tasks;

namespace ConsoleInterface.Implementations.Commands
{
    public class PassCourseCommand : BaseCommand
    {
        private IAccountService accountService;

        private ChangeAccountCourseValidator changeAccountCourseValidator;

        public PassCourseCommand(
            IAccountService accountService,
            ChangeAccountCourseValidator changeAccountCourseValidator,
            string commandName)
            : base(commandName)
        {
            this.accountService = accountService;

            this.changeAccountCourseValidator = changeAccountCourseValidator;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Passing the course");

            Console.Write("Course ID: ");

            if (!int.TryParse(Console.ReadLine(), out int courseId))
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

            if (!this.ValidateEntity(changeAccountCourse))
            {
                return;
            }

            var actionResult = await this.accountService.ChangeAccountCourseStatusAsync(changeAccountCourse);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine(actionResult.MessageCode);
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
                Console.WriteLine(string.Join("\n", validationresult.Errors));
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}
