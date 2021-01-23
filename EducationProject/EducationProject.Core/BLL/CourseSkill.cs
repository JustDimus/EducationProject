using System;
using System.Collections.Generic;
using System.Text;
using EducationProject.Core.DAL;

namespace EducationProject.Core.BLL
{
    public class CourseSkill: BaseEntity
    {
        public Skill Skill { get; set; }

        public int SkillChange { get; set; }
    }
}
