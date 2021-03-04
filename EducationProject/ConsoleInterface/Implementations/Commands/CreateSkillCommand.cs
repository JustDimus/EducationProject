using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleInterface.Validators;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateSkillCommand : BaseCommand
    {
        private ISkillService skillService;

        private ChangeEntityValidator<SkillDTO> changeEntityValidator;

        public CreateSkillCommand(ISkillService skillService,
            ChangeEntityValidator<SkillDTO> changeEntityValidator,
            string commandName)
            : base(commandName)
        {
            this.skillService = skillService;

            this.changeEntityValidator = changeEntityValidator;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Creating new skill");

            Console.Write("Title: ");

            var title = Console.ReadLine();

            Console.Write("Description: ");

            var description = Console.ReadLine();

            Console.Write("Max value: ");

            if(!Int32.TryParse(Console.ReadLine(), out int maxValue))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var changeEntity = new ChangeEntityDTO<SkillDTO>()
            {
                AccountId = accountId,
                Entity = new SkillDTO()
                {
                    Title = title,
                    MaxValue = maxValue,
                    Description = description
                }
            };

            if(!this.ValidateEntity(changeEntity))
            {
                return;
            }

            var actionResult = await this.skillService.CreateAsync(changeEntity);

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

        private bool ValidateEntity(ChangeEntityDTO<SkillDTO> changeEntity)
        {
            var validationresult = this.changeEntityValidator.Validate(changeEntity);

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
