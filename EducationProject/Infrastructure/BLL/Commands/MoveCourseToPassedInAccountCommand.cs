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
    public class MoveCourseToPassedInAccountCommand : ICommand
    {
        public string Name => "MoveCourseToPassed";

        private IMapping<CourseBO> _courses;

        private IMapping<AccountBO> _accounts;

        public MoveCourseToPassedInAccountCommand(IMapping<CourseBO> courses, IMapping<AccountBO> accounts)
        {
            _courses = courses;
            _accounts = accounts;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            int? accountId = Params[1] as int?;

            int? courseId = Params[2] as int?;

            if (account is null || accountId is null || courseId is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data: MoveCourseToPassedCommand"
                };
            }

            if (account.AccountData.Id != accountId)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Only account owner can add courses to it's account: MoveCourseToPassedCommand"
                };
            }

            var acc = _accounts.Get(accountId.GetValueOrDefault());

            var course = _courses.Get(courseId.GetValueOrDefault());

            if(acc.CoursesInProgress.Any(c => c.Id == courseId) == false 
                || acc.PassedCourses.Any(c => c.Id == course.Id))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Can't move course: MoveCourseToPassedCommand"
                };
            }

            acc.CoursesInProgress = acc.CoursesInProgress.Where(p => p.Id != course.Id);

            acc.PassedCourses = acc.PassedCourses.Append(course);

            _accounts.Update(acc);

            _accounts.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"In account {account.AccountData.Email} course '{course.Title}' changed status to 'passed'"
            };
        }
    }
}
