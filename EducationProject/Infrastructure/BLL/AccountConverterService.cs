using EducationProject.Core.BLL;
using EducationProject.Core.PL;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL
{
    public class AccountConverterService
    {
        private IMapping<AccountBO> _accounts;

        public AccountConverterService(IMapping<AccountBO> accounts)
        {
            _accounts = accounts;
        }

        public AccountPL ConvertBLLToPL(AccountBO account)
        {
            return new AccountPL()
            {
                Id = account.Id,
                RegistrationDate = account.RegistrationDate,
                CoursesInProgress = account.CoursesInProgress,
                Email = account.Email,
                FirstName = account.FirstName,
                PassedCourses = account.PassedCourses,
                PhoneNumber = account.PhoneNumber,
                SecondName = account.SecondName,
                SkillResults = account.SkillResults
            };
        }

        public IEnumerable<AccountPL> ConvertBLLToPL(IEnumerable<AccountBO> accounts)
        {
            return accounts.Select(a => ConvertBLLToPL(a));
        }
    }
}
