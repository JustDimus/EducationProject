using EducationProject.BLL.Interfaces;
using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
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

        private IMapping<CourseDBO> courses;

        private IMapping<AccountDBO> accounts;

        public MoveCourseToPassedInAccountCommand(IMapping<CourseDBO> courseMapping, IMapping<AccountDBO> accountMapping)
        {
            courses = courseMapping;
            accounts = accountMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData account = Params[0] as AccountAuthenticationData;

            int? accountId = Params[1] as int?;

            int? courseId = Params[2] as int?;

            if (account is null || accountId.HasValue == false || courseId.HasValue == false)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data: MoveCourseToPassedCommand"
                };
            }

            if (account.AccountId != accountId)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Only account owner can add courses to it's account: MoveCourseToPassedCommand"
                };
            }

            if(accounts.Any(a => a.AccountCourses.Any(ac => ac.CourseId == courseId
            && ac.AccountId == accountId
            && ac.Status == EducationProject.Core.DAL.EF.Enums.ProgressStatus.InProgress)) == false)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Can't move course: MoveCourseToPassedCommand"
                };
            }

            accounts.Get(accountId.Value).AccountCourses.Where(ac => ac.CourseId == courseId.Value)
                .FirstOrDefault().Status = EducationProject.Core.DAL.EF.Enums.ProgressStatus.Passed;

            accounts.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"In account {account.AccountId} course 'Id: {courseId}' changed status to 'passed'"
            };
        }
    }
}
