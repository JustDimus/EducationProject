﻿using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class ShowCourseInfoCommand : ICommand
    {
        public string Name => "_getCourseInfo";

        private ICourseService courses;

        public ShowCourseInfoCommand(ICourseService courseService)
        {
            this.courses = courseService;
        }

        public void Run(ref string token)
        {
            int courseId = 0;

            Console.WriteLine("Showing course data");

            Console.Write("Course ID: ");

            if (Int32.TryParse(Console.ReadLine(), out courseId) == false)
            {
                Console.WriteLine("Error. Enter the number!");
                Console.WriteLine();
                return;
            }

            var course = courses.GetCourseInfo(courseId);

            if(course is null)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
                return;
            }

            StringBuilder builder = new StringBuilder();

            builder.Append($"ID: {course.Id} {course.Title}\n");
            builder.Append($"Description: {course.Description}\n");
            builder.Append($"Skills: \n\t");
            builder.Append(String.Join("\n\t", course.Skills
                .Select(s => $"ID: {s.SkillId} Title: {s.SkillTitle} Change: {s.SkillChange}")));
            builder.Append("\nMaterials: \n\t");
            builder.Append(String.Join("\n\t", course.Materials
                .Select(m => $"ID: {m.MaterialId} Title: {m.MaterialTitle}")));

            Console.WriteLine(builder);

            Console.WriteLine();
        }
    }
}
