using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddMaterialToCourseCommand : BaseCommand
    {
        private ICourseService courseService;

        private ChangeCourseMaterialValidator changeCourseMaterialValidator;

        public AddMaterialToCourseCommand(ICourseService courseService,
            ChangeCourseMaterialValidator changeCourseMaterialValidator,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.changeCourseMaterialValidator = changeCourseMaterialValidator;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Adding skill to course");

            Console.Write("Course ID: ");

            if (!Int32.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("Material ID: ");

            if (!Int32.TryParse(Console.ReadLine(), out int materialId))
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

            if(!this.ValidateEntity(changeCourseMaterial))
            {
                return;
            }

            var actionResult = await this.courseService.AddCourseMaterialAsync(changeCourseMaterial);

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

        private bool ValidateEntity(ChangeCourseMaterialDTO changeCourseMaterial)
        {
            var validationresult = this.changeCourseMaterialValidator.Validate(changeCourseMaterial);

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
