using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class Account : BaseEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

        public List<AccountCourse> AccountCourses { get; set; }

        public List<AccountMaterial> AccountMaterials { get; set; }

        public List<AccountSkill> AccountSkills { get; set; }

        public List<Course> CreatedCourses { get; set; }
    }
}
