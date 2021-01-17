using DomainCore.CourseInfo.Interfaces;
using DomainCore.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.CourseInfo
{
    public class CourseAchievementUp: ISkillUp
    {
        public Achievement Achievement { get; set; }

        public int LvlChanging { get; set; }
    }
}
