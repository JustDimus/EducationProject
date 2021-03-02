using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetAccountCourses : ICommand
    {
        public string Name => "_getMyCourses";
        
        private ICourseService courses;

        private int pageSize;

        public GetAccountCourses(ICourseService courseService, int defaultPageSize)
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

            var coursesData = courses.GetMyCourses(new GetCoursesByCreator()
            {
                Token = token,
                PageInfo = new PageInfoDTO()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber
                }
            });

            if(coursesData is null)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
                return;
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
