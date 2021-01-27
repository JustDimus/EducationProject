using EducationProject.Core.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.PL
{
    public class AccountPL
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<CourseBO> CoursesInProgress { get; set; }

        public IEnumerable<CourseBO> PassedCourses { get; set; }

        public IEnumerable<AccountSkillBO> SkillResults { get; set; }
    }
}
