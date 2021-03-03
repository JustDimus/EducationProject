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
        private ICourseService courseService;

        public ChangeCourseStateCommand(ICourseService courseService,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Changing course state");

            Console.Write("Course ID: ");

            if (int.TryParse(Console.ReadLine(), out int courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("New state 0 - hide, 1 - publish: ");

            if(int.TryParse(Console.ReadLine(), out int state) == false)
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

            var actionResult = this.courseService.ChangeCourseVisibility(new CourseVisibilityDTO()
            {
                Token = token,
                CourseId = courseId,
                Visibility = state == 0 ? false : true
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
