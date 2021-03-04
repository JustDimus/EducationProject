using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class ShowCourseInfoCommand : BaseCommand
    {
        private ICourseService courseService;

        public ShowCourseInfoCommand(
            ICourseService courseService,
            string commandName)
            : base(commandName)
        {
            this.courseService = courseService;
        }

        public async override void Run(int accountId)
        {
            Console.WriteLine("Showing course data");

            Console.Write("Course ID: ");

            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var actionResult = await this.courseService.GetCourseInfoAsync(courseId);

            if (!actionResult.IsSuccessful)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
                return;
            }

            StringBuilder builder = new StringBuilder();

            var course = actionResult.Result;

            builder.Append($"ID: {course.Id} {course.Title}\n");
            builder.Append($"Description: {course.Description}\n");
            builder.Append($"Skills: \n\t");
            builder.Append(string.Join(
                "\n\t", 
                course.Skills.Select(s => $"ID: {s.SkillId} Title: {s.SkillTitle} Change: {s.SkillChange}")));
            builder.Append("\nMaterials: \n\t");
            builder.Append(string.Join(
                "\n\t", 
                course.Materials.Select(m => $"ID: {m.MaterialId} Title: {m.MaterialTitle}")));

            Console.WriteLine(builder);
            Console.WriteLine();
        }
    }
}
