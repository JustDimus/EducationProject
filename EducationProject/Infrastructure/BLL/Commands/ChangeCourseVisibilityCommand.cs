using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL.Commands
{
    public class ChangeCourseVisibilityCommand : ICommand
    {
        public string Name => "ChangeCourseVisibility";

        private IMapping<CourseBO> _courses;

        public ChangeCourseVisibilityCommand(IMapping<CourseBO> courses)
        {
            _courses = courses;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            int? courseId = Params[1] as int?;

            bool? newState = Params[2] as bool?;

            if(account is null || courseId is null || newState is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data: ChangeCourseVisibilityCommand"
                };
            }

            CourseBO course = _courses.Get(courseId.GetValueOrDefault());

            if (course is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = "UndefinedError: ChangeCourseVisibilityCommand"
                };
            }

            if (account.AccountData.Id != course.CreatorId)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Only course's owner can change course: AddExistingCourseToAccountCommand"
                };
            }

            if(course.Materials.Any() == false)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Course must have at least 1 material for publication"
                };
            }

            course.IsVisible = newState.GetValueOrDefault();

            _courses.Update(course);

            _courses.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Succesfully changed course visibility state to: {course.IsVisible}"
            };
        }
    }
}
