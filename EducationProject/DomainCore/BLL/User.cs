using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.BLL
{
    public class User : BaseEntity
    {
        public string Mail { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Password { get; set; }
    }
}
