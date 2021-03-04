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
    public class AddMaterialToCourseCommand : BaseCommand
    {
        private ICourseService courseService;

        private ChangeCourseMaterialValidator changeCourseMaterialValidator;

        public AddMaterialToCourseCommand(
            ICourseService courseService,
            ChangeCourseMaterialValidator changeCourseMaterialValidator,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.changeCourseMaterialValidator = changeCourseMaterialValidator;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Adding skill to course");

            Console.Write("Course ID: ");

            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("Material ID: ");

            if (!int.TryParse(Console.ReadLine(), out int materialId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var changeCourseMaterial = new ChangeCourseMaterialDTO()
            {
                AccountId = accountId,
                CourseId = courseId,
                MaterialId = materialId
            };

            if (!this.ValidateEntity(changeCourseMaterial))
            {
                return;
            }

            var actionResult = await this.courseService.AddCourseMaterialAsync(changeCourseMaterial);

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

        private bool ValidateEntity(ChangeCourseMaterialDTO changeCourseMaterial)
        {
            var validationresult = this.changeCourseMaterialValidator.Validate(changeCourseMaterial);

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
