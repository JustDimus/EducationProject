using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
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

        private IMapping<CourseDBO> courses;

        public CreateCourseCommand(IMapping<CourseDBO> courseMapping)
        {
            courses = courseMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData authData = Params[0] as AccountAuthenticationData;

            string courseName = Params[1] as string;

            string courseDescription = Params[2] as string;

            if(string.IsNullOrEmpty(courseName) || string.IsNullOrEmpty(courseDescription) || authData is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid course data: CreateAccountCommand"
                };
            }

            var course = new CourseDBO()
            {
                CreatorId = authData.AccountId,
                Description = courseDescription,
                IsVisible = false,
                Title = courseName
            };

            courses.Create(course);

            courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Created course by account {authData.AccountId} with name {course.Title}. Id: {course.Id}"
            };
        }
    }
}
