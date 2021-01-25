using EducationProject.Core.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.PL
{
    public class Account
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<Course> CoursesInProgress { get; set; }

        public IEnumerable<Course> PassedCourses { get; set; }

        public IEnumerable<AccountSkill> SkillResults { get; set; }
    }
}
