using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL
{
    public class Account: BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationData { get; set; }

        public string PhoneNumber { get; set; }
    }
}
