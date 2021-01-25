using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class CreateCourseCommand : ICommand
    {
        public string Name => "CreateCourse";

        private IMapping<Course> _courses;

        public CreateCourseCommand(IMapping<Course> AccMapping)
        {
            _courses = AccMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            string courseName = Params[1] as string;

            string courseDescription = Params[2] as string;

            if(string.IsNullOrEmpty(courseName) || string.IsNullOrEmpty(courseDescription))
            {
                return new OperationResult()
                {
                    Status = ResultType.Error,
                    Result = $"Invalid course data: CreateAccountCommand"
                };
            }

            var course = new Course()
            {
                CreatorId = account.AccountData.Id,
                Description = courseDescription,
                IsVisible = false,
                Title = courseName
            };

            _courses.Create(course);

            _courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created course by account {account.AccountData.Id} with name {course.Title}. Id: {course.Id}"
            };
        }
    }
}
