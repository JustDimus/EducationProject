using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class Skill: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxValue { get; set; }

        public List<CourseSkill> CourseSkills { get; set; }

        public List<AccountSkill> AccountSkills { get; set; }
    }
}
