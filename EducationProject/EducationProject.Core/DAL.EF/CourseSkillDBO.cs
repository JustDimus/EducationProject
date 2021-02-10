using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class CourseSkillDBO
    {
        public int CourseId { get; set; }

        public CourseDBO Course { get; set; }

        public int SkillId { get; set; }

        public SkillDBO Skill { get; set; }

        public int Change { get; set; }
    }
}
