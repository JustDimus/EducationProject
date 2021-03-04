using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class CourseSkill
    {
        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int Change { get; set; }
    }
}
