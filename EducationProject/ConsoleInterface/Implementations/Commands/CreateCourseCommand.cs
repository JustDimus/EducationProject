using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateCourseCommand : BaseCommand
    {
        private ICourseService courseService;

        public CreateCourseCommand(ICourseService courseService,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Creating new course");

            Console.Write("Title: ");

            var title = Console.ReadLine();

            Console.Write("Description: ");

            var description = Console.ReadLine();

            if (String.IsNullOrEmpty(title) == true || String.IsNullOrEmpty(description) == true)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var actionResult = this.courseService.Create(new ChangeEntityDTO<ShortCourseInfoDTO>()
            {
                Token = token,
                Entity = new ShortCourseInfoDTO()
                {
                    Title = title,
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
