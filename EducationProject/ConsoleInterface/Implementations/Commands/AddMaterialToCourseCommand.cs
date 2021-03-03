using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddMaterialToCourseCommand : BaseCommand
    {
        private ICourseService courseService;

        public AddMaterialToCourseCommand(ICourseService courseService,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Adding skill to course");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out int courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("Material ID: ");

            if (Int32.TryParse(Console.ReadLine(), out int materialId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = this.courseService.AddCourseMaterial(new ChangeCourseMaterialDTO()
            {
                Token = token,
                CourseId = courseId,
                MaterialId = materialId
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
