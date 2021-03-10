using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class FullAccountInfoDTO : ShortAccountInfoDTO
    {
        public int PassedCoursesCount { get; set; }

        public EntityInfoPageDTO<AccountCourseDTO> Courses { get; set; }

        public EntityInfoPageDTO<AccountSkillDTO> Skills { get; set; }
    }
}
