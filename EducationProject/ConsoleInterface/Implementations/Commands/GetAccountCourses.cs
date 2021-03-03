﻿using ConsoleInterface.Interfaces;
using EducationProject.BLL.Interfaces;
using EducationProject.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleInterface.Implementations.Commands
{
    public class GetAccountCourses : BaseCommand
    {        
        private ICourseService courseService;

        private int pageSize;

        public GetAccountCourses(ICourseService courseService, 
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

            var coursesData = this.courseService.GetMyCourses(new GetCoursesByCreator()
            {
                Token = token,
                PageInfo = new PageInfoDTO()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber
                }
            });

            if(coursesData == null)
            {
                Console.WriteLine("Error");
                Console.WriteLine();
                return;
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendJoin("\n",
                coursesData.Select(c => $"{c.Id}: {c.Title}.\n\tDescription: {c.Description}"));

            Console.WriteLine(builder);
            Console.WriteLine();
        }
    }
}
