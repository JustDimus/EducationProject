using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateSkillCommand : BaseCommand
    {
        private ISkillService skillService;

        public CreateSkillCommand(ISkillService skillService,
            string commandName)
            : base(commandName)
        {
            this.skillService = skillService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Creating new skill");

            Console.Write("Title: ");

            var title = Console.ReadLine();

            Console.Write("Description: ");

            var description = Console.ReadLine();

            Console.Write("Max value: ");

            if(Int32.TryParse(Console.ReadLine(), out int maxValue) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = this.skillService.Create(new ChangeEntityDTO<SkillDTO>()
            {
                Token = token,
                Entity = new SkillDTO()
                {
                    Title = title,
                    MaxValue = maxValue,
                    Description = description
                }
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
