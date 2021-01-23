using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core
{
    public class CourseSkill: BaseEntity
    {
        public int CourseId { get; set; }

        public int SkillId { get; set; }

        public int SkillChange { get; set; }
    }
}
