using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class ChangeCourseStateCommand : BaseCommand
    {
        private ICourseService courses;

        public ChangeCourseStateCommand(ICourseService courseService,
            string commandName)
            : base(commandName)
        {
            this.courses = courseService;
        }

        public override void Run(ref string token)
        {
            int courseId = 0;

            int state = 0;

            Console.WriteLine("Changing course state");

            Console.Write("Course ID: ");

            if (int.TryParse(Console.ReadLine(), out courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("New state 0 - hide, 1 - publish: ");

            if(int.TryParse(Console.ReadLine(), out state) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if(state != 0 & state != 1)
            {
                Console.WriteLine("Error. Enter the 0 or 1");
                Console.WriteLine();
                return;
            }

            if(courses.ChangeCourseVisibility(new CourseVisibilityDTO()
            {
                Token = token,
                CourseId = courseId,
                Visibility = state == 0? false : true
            }) == false)
            {
                Console.WriteLine("Error.");
            }
            else
            {
                Console.WriteLine("Successful");
            }

            Console.WriteLine();
        }
    }
}
