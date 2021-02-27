using EducationProject.BLL.Interfaces;
using EducationProject.Core.DAL.EF;
using Infrastructure.DAL.EF.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EducationProject.BLL.Models;
using CourseStatus = EducationProject.Core.DAL.EF.Enums.ProgressStatus;

namespace Infrastructure.BLL.Services
{
    /*
    public class AccountService : BaseService<AccountDBO>
    {
        
        public AccountService(BaseRepository<AccountDBO> accounts, 
            AuthorizationService authService)
            : base(accounts, authService)
        {

        }

        public bool Create(CreateAccountDTO accountData)
        {
            if (String.IsNullOrEmpty(accountData.Email) || String.IsNullOrEmpty(accountData.Password))
            {
                return false;
            }

            if(entity.Any(a => a.Email == accountData.Email))
            {
                return false;
            }

            AccountDBO account = new AccountDBO()
            {
                Email = accountData.Email,
                Password = accountData.Password,
                RegistrationDate = DateTime.Now
            };

            entity.Create(account);

            entity.Save();

            return true;
        }

        public AccountPL Get(int id)
        {
            throw new Exception();
            /*
            return entity.Get(a => a.Id == id, a => new AccountPL()
            {
                Email = a.Email,
                Id = a.Id,
                FirstName = a.FirstName,
                PhoneNumber = a.PhoneNumber,
                RegistrationDate = a.RegistrationDate,
                SecondName = a.SecondName,
                CoursesInProgress = a.AccountCourses.Where(ac => ac.Status == CourseStatus.InProgress)
                .Select(ac => ac.Course).Skip(0).Take(10).Select(c => new EducationProject.Core.BLL.CourseBO()
                {
                    Id = c.Id,
                    Description = c.Description,
                    CreatorId
                })
            });
            
        }

        public bool Update(UpdateAccountDTO accountData)
        {
            int accountId = authService.AuthenticateAccount(accountData.Token);

            if(accountId == 0 || accountData.Id != accountId)
            {
                return false;
            }

            AccountDBO account = entity.Get(a => a.Id == accountId);

            account.FirstName = accountData.FirstName;
            account.SecondName = accountData.SecondName;
            account.PhoneNumber = accountData.PhoneNumber;

            entity.Save();

            return true;
        }

        public bool Delete(string token)
        {
            int accountId = authService.AuthenticateAccount(token);

            if(accountId == 0)
            {
                return false;
            }

            entity.Delete(accountId);

            entity.Save();

            authService.DeauthorizeAccount(token);

            return true;
        }

        public bool AddNewCourseToAccount(string token, int accountId, int courseId)
        {
            throw new Exception();
        }
    }
*/
}
