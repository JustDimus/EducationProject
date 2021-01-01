using Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var uow = new UnitOfWork(new XMLDataContext.DataContext.XMLContext());

            var t = new DomainCore.BLL.User()
            {
                Id = 15,
                City = "New-York",
                Country = "USA",
                FirstName = "John",
                SecondName = "Cena",
                Mail = "JohnCena@gmail.com",
                PhoneNumber = "+10777123456"
            };

            var z = new DomainCore.BLL.User()
            {
                Id = 15,
                City = "Las-Vegas",
                Country = "USA",
                FirstName = "John",
                SecondName = "Cena",
                Mail = "JohnCena@gmail.com",
                PhoneNumber = "+10777123456"
            };

            uow.Users.Insert(z);

            uow.Users.Update(t);

            uow.Save();

            var u = uow.Users.Get(t => true).ToList();

        }
    }
}
