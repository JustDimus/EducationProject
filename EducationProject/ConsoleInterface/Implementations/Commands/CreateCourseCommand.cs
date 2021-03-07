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
    public class CreateCourseCommand : BaseCommand
    {
        private ICourseService courseService;

        private ChangeEntityValidator<ShortCourseInfoDTO> changeEntityValidator;

        public CreateCourseCommand(
            ICourseService courseService,
            ChangeEntityValidator<ShortCourseInfoDTO> changeEntityValidator,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.changeEntityValidator = changeEntityValidator;
        }

        public async override Task Run(int accountId)
        {
            Console.WriteLine("Creating new course");

            Console.Write("Title: ");

            var title = Console.ReadLine();

            Console.Write("Description: ");

            var description = Console.ReadLine();

            var changeEntity = new ChangeEntityDTO<ShortCourseInfoDTO>()
            {
                AccountId = accountId,
                Entity = new ShortCourseInfoDTO()
                {
                    Title = title,
                    Description = description
                }
            };

            if (!this.ValidateEntity(changeEntity))
            {
                return;
            }

            var actionResult = await this.courseService.CreateAsync(changeEntity);

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

        private bool ValidateEntity(ChangeEntityDTO<ShortCourseInfoDTO> changeEntity)
        {
            var validationresult = this.changeEntityValidator.Validate(changeEntity);

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
