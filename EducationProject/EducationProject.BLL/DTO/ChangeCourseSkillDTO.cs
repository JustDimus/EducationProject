using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.DTO
{
    public class ChangeCourseSkillDTO : AccountIdBasedDTO
    {
        public int CourseId { get; set; }

        public int SkillId { get; set; }

        public int Change { get; set; }
    }
}
