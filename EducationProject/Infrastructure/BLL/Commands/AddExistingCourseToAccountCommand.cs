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
    public class AddExistingCourseToAccountCommand : ICommand
    {
        public string Name => "AddExistingCourseToAccount";

        private IMapping<CourseBO> _courses;

        private IMapping<AccountBO> _accounts;

        public AddExistingCourseToAccountCommand(IMapping<CourseBO> courses, IMapping<AccountBO> accounts)
        {
            _accounts = accounts;

            _courses = courses;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            int? accountId = Params[1] as int?;

            int? courseId = Params[2] as int?;

            if(account is null || accountId is null || courseId is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data: AddExistingCourseToAccountCommand"
                };
            }

            if(account.AccountData.Id != accountId)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Only account's owner can add courses to account: AddExistingCourseToAccountCommand"
                };
            }

            var acc = _accounts.Get(accountId.GetValueOrDefault());

            var course = _courses.Get(courseId.GetValueOrDefault());

            if(acc.CoursesInProgress.Any(c => c.Id == course.Id) || acc.PassedCourses.Any(c => c.Id == course.Id))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Course already exists in such account: AddExistingCourseToAccountCommand"
                };
            }

            acc.CoursesInProgress = acc.CoursesInProgress.Append(course);

            _accounts.Update(acc);

            _accounts.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Added course '{course.Title}' to account '{acc.Email}'"
            };
        }
    }
}
