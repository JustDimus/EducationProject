using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class AccountDBO : BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

        public List<AccountCourseDBO> AccountCourses { get; set; }

        public List<AccountMaterialDBO> AccountMaterials { get; set; }
    }
}
