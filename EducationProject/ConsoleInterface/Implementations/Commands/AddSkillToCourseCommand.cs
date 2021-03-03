using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class AddSkillToCourseCommand : BaseCommand
    {
        private ICourseService courseService;

        public AddSkillToCourseCommand(ICourseService courseService,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Adding skill to course");

            Console.Write("Course ID: ");

            if(Int32.TryParse(Console.ReadLine(), out int courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            Console.Write("Skill ID: ");

            if (Int32.TryParse(Console.ReadLine(), out int skillId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }
            
            Console.Write("Skill change: ");

            if (Int32.TryParse(Console.ReadLine(), out int change) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = this.courseService.AddCourseSkill(new ChangeCourseSkillDTO()
            {
                Change = change,
                Token = token,
                CourseId = courseId,
                SkillId = skillId
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
