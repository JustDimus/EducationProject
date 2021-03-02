using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetCoursesCommand : ICommand
    {
        public string Name => "_getCourses";

        private ICourseService courses;

        private int pageSize;

        public GetCoursesCommand(ICourseService courseService, int defaultPageSize)
        {
            this.courses = courseService;

            this.pageSize = defaultPageSize;
        }

        public void Run(ref string token)
        {
            int pageNumber = 0;

            Console.WriteLine("Getting courses");

            Console.Write("Enter the page: ");

            Int32.TryParse(Console.ReadLine(), out pageNumber);

            var coursesData = courses.Get(new PageInfoDTO()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            StringBuilder builder = new StringBuilder();

            foreach (var course in coursesData)
            {
                builder.Append($"{course.Id}: {course.Title}.\n\tDescription: {course.Description}\n");
            }

            Console.WriteLine(builder);
        }
    }
}
