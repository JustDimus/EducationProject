using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class CreateCourseCommand : ICommand
    {
        public string Name => "_createCourse";

        private ICourseService courses;

        public CreateCourseCommand(ICourseService courseService)
        {
            courses = courseService;
        }

        public void Run(ref string token)
        {
            string title = null;

            string description = null;

            Console.WriteLine("Creating new course");

            Console.Write("Title: ");

            title = Console.ReadLine();

            Console.Write("Description: ");

            description = Console.ReadLine();

            if(courses.Create(new ChangeEntityDTO<ShortCourseInfoDTO>()
            {
                Token = token,
                Entity = new ShortCourseInfoDTO()
                {
                    Title = title,
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
