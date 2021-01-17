using DomainCore.CourseInfo.Interfaces;
using DomainCore.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.CourseInfo
{
    public class CourseSkillUp: ISkillUp
    {
        public Skill Skill { get; set; }

        public float SkillChange { get; set; }
    }
}
