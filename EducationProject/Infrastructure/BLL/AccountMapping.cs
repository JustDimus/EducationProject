using EducationProject.BLL.Interfaces;
using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.BLL;

namespace Infrastructure.BLL
{
    class AccountMapping: IMapping<Account>
    {
        private UnitOfWork _uow; 

        public AccountMapping(UnitOfWork UOW)
        {
            _uow = UOW;
        }

        public void Create(Account Entity)
        {
            var account = new EducationProject.Core.DAL.Account()
            {
                Email = Entity.Email,
                FirstName = Entity.FirstName,
                RegistrationData = Entity.RegistrationData,
                Id = Entity.Id,
                Password = Entity.Password,
                PhoneNumber = Entity.Password,
                SecondName = Entity.SecondName
            };

            _uow.Accounts.Create(account);

            Entity.Id = account.Id;

            foreach(var course in Entity.PassedCourses)
            {
                _uow.AccountCourses.Create(new EducationProject.Core.DAL.AccountCourse()
                {
                    AccountId = Entity.Id,
                    CourseId = course.Id,
                    Status = "Passed"
                });
            }
        }

        public void Delete(Account Entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Predicate<Account> Condition)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> Get(Predicate<Account> Condition)
        {
            throw new NotImplementedException();
        }

        public Account Get(int Id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Account Entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Account Entity, Predicate<Account> Condition)
        {
            throw new NotImplementedException();
        }
    }
}
