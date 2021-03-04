using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class FullAccountInfoDTO : ShortAccountInfoDTO
    {
        public int PassedCoursesCount { get; set; }

        public IEnumerable<AccountCourseDTO> PassedCourses { get; set; }

        public int CoursesInProgressCount { get; set; }

        public IEnumerable<AccountCourseDTO> CoursesInProgress { get; set; }

        public IEnumerable<AccountSkillDTO> AccountSkills { get; set; }
    }
}
