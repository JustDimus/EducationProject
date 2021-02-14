using EducationProject.Core.BLL;
using EducationProject.Core.DAL.EF;
using EducationProject.Core.PL;
using EducationProject.Core.PL.EF;
using EducationProject.DAL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BLL
{
    public class AccountConverterService
    {
        private IMapping<AccountDBO> accounts;

        public AccountConverterService(IMapping<AccountDBO> accountMapping)
        {
            accounts = accountMapping;
        }

        public AccountPL ConvertBLLToPL(AccountDBO account)
        {
            return new AccountPL()
            {
                Id = account.Id,
                RegistrationDate = account.RegistrationDate,
                //CoursesInProgress = account.CoursesInProgress,
                Email = account.Email,
                FirstName = account.FirstName,
                //PassedCourses = account.PassedCourses,
                PhoneNumber = account.PhoneNumber,
                SecondName = account.SecondName,
                //SkillResults = account.SkillResults
            };
        }


        public IEnumerable<AccountPL> ConvertBLLToPL(IEnumerable<AccountDBO> accounts)
        {
            return accounts.Select(a => ConvertBLLToPL(a));
        }
    }
}
