using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public class AccountBO: DAL.AccountDBO
    {
        public IEnumerable<CourseBO> CoursesInProgress { get; set; }

        public IEnumerable<CourseBO> PassedCourses { get; set; }

        public IEnumerable<AccountSkillBO> SkillResults { get; set; }
    }
}
