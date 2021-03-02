using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateSkillCommand : ICommand
    {
        public string Name => "_createSkill";

        private ISkillService skills;

        public CreateSkillCommand(ISkillService skillService)
        {
            skills = skillService;
        }

        public void Run(ref string token)
        {
            string title = null;

            string description = null;

            int maxValue = 0;

            Console.WriteLine("Creating new skill");

            Console.Write("Title: ");

            title = Console.ReadLine();

            Console.Write("Description: ");

            description = Console.ReadLine();

            Console.Write("Max value: ");

            if(Int32.TryParse(Console.ReadLine(), out maxValue) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if(skills.Create(new ChangeEntityDTO<SkillDTO>()
            {
                Token = token,
                Entity = new SkillDTO()
                {
                    Title = title,
                    MaxValue = maxValue,
                    Description = description
                }
            }) == true)
            {
                Console.WriteLine("Successful");
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.WriteLine();
        }
    }
}
