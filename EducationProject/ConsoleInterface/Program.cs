using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using XMLDataContext.DataContext;
using XMLDataContext.DataSets;
using EducationProject.Core.DAL;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
          //  var uow = new UnitOfWork();

            var t = new XMLDataContext.DataContext.XMLContext();

            t.Entity<Account>().Create(new Account()
            {
                Email = "Hello@Mail.ru",
                FirstName = "Jonathan",
                SecondName = "Joestar"
            });

            t.Save();
            /*
            t.Accounts.Create(new Account()
            {
                Email = "dssadds",
                FirstName = "dsad"
            });

            t.AccountSkills.Create(new AccountSkills()
            {
                CurrentResult = 15,
                Id = 1,
                Level = 2,
                Skill = 123
            });

            t.AccountSkills.Delete(t => true);

            //t.Accounts.Delete(t => true);

            t.Accounts.Save();
            */
            t.Save();
        }
    }
}
