using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddSkillToCourseCommand : ICommand
    {
        public string Name => "_addSkillToCourse";

        private ICourseService courses;

        public AddSkillToCourseCommand(ICourseService courseService)
        {
            this.courses = courseService;
        }

        public void Run(ref string token)
        {
            int courseId = 0;

            int skillId = 0;

            int change = 0;

            Console.WriteLine("Adding skill to course");

            Console.Write("Course ID: ");

            if(Int32.TryParse(Console.ReadLine(), out courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("Skill ID: ");

            if (Int32.TryParse(Console.ReadLine(), out skillId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }
            
            Console.Write("Skill change: ");

            if (Int32.TryParse(Console.ReadLine(), out change) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            if(courses.AddCourseSkill(new ChangeCourseSkillDTO()
            {
                Change = change,
                Token = token,
                CourseId = courseId,
                SkillId = skillId
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
