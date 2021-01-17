using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.AccountData
{
    public class UserInformation
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
