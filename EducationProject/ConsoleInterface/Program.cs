using Infrastructure.UOW;
using System;

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
                City = "Counter",
                Country = "Cww",
                FirstName = "John",
                SecondName = "Cena",
                Mail = "JohnCena@gmail.com",
                PhoneNumber = "+380777123456"
            };

            var z = new DomainCore.BLL.User()
            {
                Id = 15,
                City = "Counter",
                Country = "Cww",
                FirstName = "John",
                SecondName = "Cena",
                Mail = "JohnCena@gmail.com",
                PhoneNumber = "+380777123456"
            };

            uow.Users.Insert(t);
            uow.Users.Update(z);
        }
    }
}
