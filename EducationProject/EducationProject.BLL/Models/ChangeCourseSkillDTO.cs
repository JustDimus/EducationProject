using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Models
{
    public class ChangeCourseSkillDTO : TokenBasedDTO
    {
        public int CourseId { get; set; }

        public int SkillId { get; set; }

        public int Change { get; set; }
    }
}
