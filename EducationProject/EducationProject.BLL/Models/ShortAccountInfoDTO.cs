using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ShortAccountInfoDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

    }
}
