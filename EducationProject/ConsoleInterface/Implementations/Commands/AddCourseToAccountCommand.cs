using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;
using Infrastructure.BLL;
using System.Threading.Tasks;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddCourseToAccountCommand : BaseCommand
    {
        private IAccountService accountService;

        private ChangeAccountCourseValidator changeAccountCourseValidator;

        public AddCourseToAccountCommand(
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
            Console.WriteLine("Adding course to account");

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
                CourseId = courseId
            };

            if (!this.ValidateEntity(changeAccountCourse))
            {
                return;
            }

            var actionResult = await this.accountService.AddAccountCourseAsync(changeAccountCourse);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine(actionResult.ResultMessage);
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }

        private bool ValidateEntity(ChangeAccountCourseDTO changeAccountCourse)
        {
            var validationResult = this.changeAccountCourseValidator.Validate(changeAccountCourse);

            if (!validationResult.IsValid)
            {
                Console.WriteLine(string.Join("\n", validationResult.Errors));
                Console.WriteLine();

                return false;
            }

            return true;
        }
    }
}
