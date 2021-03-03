using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetCoursesCommand : BaseCommand
    {
        private ICourseService courseService;

        private int pageSize;

        public GetCoursesCommand(ICourseService courseService, 
            int defaultPageSize, string commandName)
            : base(commandName)
        {
            this.courseService = courseService;

            this.pageSize = defaultPageSize;
        }

        public override void Run(ref string token)
        {
            Console.WriteLine("Getting courses");

            Console.Write("Enter the page: ");

            if (Int32.TryParse(Console.ReadLine(), out int pageNumber) == false)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            var coursesData = courseService.Get(new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            if(coursesData == null)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
            }

            StringBuilder builder = new StringBuilder();

            foreach (var course in coursesData)
            {
                builder.Append($"{course.Id}: {course.Title}.\n\tDescription: {course.Description}\n");
            }

            Console.WriteLine(builder);
        }
    }
}
