using EducationProject.BLL.Interfaces;
using EducationProject.Core.DAL.EF;
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

        private IMapping<CourseDBO> courses;

        private IMapping<AccountDBO> accounts;

        public AddExistingCourseToAccountCommand(IMapping<CourseDBO> courseMapping, IMapping<AccountDBO> accountMapping)
        {
            this.courses = courseMapping;

            this.accounts = accountMapping;
        }

        public IOperationResult Handle(object[] Params)
        {
            AccountAuthenticationData authData = Params[0] as AccountAuthenticationData;

            int? accountId = Params[1] as int?;

            int? courseId = Params[2] as int?;

            if(authData is null || accountId is null || courseId is null)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Invalid data: AddExistingCourseToAccountCommand"
                };
            }

            if(authData.AccountId != accountId)
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Only account's owner can add courses to account: AddExistingCourseToAccountCommand"
                };
            }

            if (accounts.Any(a => a.AccountCourses.Any(ac => ac.AccountId == accountId && ac.CourseId == courseId)))
            {
                return new OperationResult()
                {
                    Status = ResultType.Failed,
                    Result = $"Course already exists in such account: AddExistingCourseToAccountCommand"
                };
            }

            accounts.Get(accountId.GetValueOrDefault()).AccountCourses.Add(new AccountCourseDBO()
            { 
                AccountId = accountId.GetValueOrDefault(),
                CourseId = courseId.GetValueOrDefault(),
                Status = EducationProject.Core.DAL.EF.Enums.ProgressStatus.InProgress
            });

            accounts.Save();

            return new OperationResult()
            {
                Status = ResultType.Success,
                Result = $"Added course '{courseId}' to account '{authData.Login}'"
            };
        }
    }
}
