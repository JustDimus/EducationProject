using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class SkillDBO: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxValue { get; set; }

        public List<CourseSkillDBO> CourseSkills { get; set; }
    }
}
