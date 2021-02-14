using EducationProject.Core.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.PL.EF
{
    public class AccountPL
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string PhoneNumber { get; set; }

        public List<CoursePL> CoursesInProgress { get; set; }

        public List<CoursePL> PassedCourses { get; set; }

        public List<AccountSkillPL> SkillResults { get; set; }
    }
}
