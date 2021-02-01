using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.DAL;

namespace EducationProject.Core.BLL
{
    public class CourseSkillBO: BaseEntity
    {
        public SkillBO Skill { get; set; }

        public int SkillChange { get; set; }
    }
}
