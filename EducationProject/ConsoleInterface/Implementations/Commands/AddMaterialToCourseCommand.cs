using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddMaterialToCourseCommand : ICommand
    {
        public string Name => "_addMaterialToCourse";

        private ICourseService courses;

        public AddMaterialToCourseCommand(ICourseService courseService)
        {
            this.courses = courseService;
        }
        public void Run(ref string token)
        {
            int courseId = 0;

            int materialId = 0;

            Console.WriteLine("Adding skill to course");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("Material ID: ");

            if (Int32.TryParse(Console.ReadLine(), out materialId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if (courses.AddCourseMaterial(new ChangeCourseMaterialDTO()
            {
                Token = token,
                CourseId = courseId,
                MaterialId = materialId
            }) == false)
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
