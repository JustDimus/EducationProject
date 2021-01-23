using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public class Account: Core.DAL.Account
    {
        public IEnumerable<Course> CoursesInProgress { get; set; }

        public IEnumerable<Course> PassedCourses { get; set; }

        public IEnumerable<AccountSkill> SkillResults { get; set; }
    }
}
