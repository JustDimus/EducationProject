using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class PublishCourseCommand : ICommand
    {
        public string Name => "_publishCourse";

        private ICourseService courses;

        public PublishCourseCommand(ICourseService courseService)
        {
            this.courses = courseService;
        }

        public void Run(ref string token)
        {
            int courseId = 0;

            Console.WriteLine("Publishing the course");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if (courses.ChangeCourseVisibility(new CourseVisibilityDTO()
            {
                Token = token,
                Visibility = true,
                CourseId = courseId
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
