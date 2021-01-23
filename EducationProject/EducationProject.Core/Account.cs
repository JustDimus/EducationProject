using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core
{
    public class Account: BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationData { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<AccountCourse> PassedCourses { get; set; }

        public IEnumerable<AccountSkills> SkillResults { get; set; }
    }
}
