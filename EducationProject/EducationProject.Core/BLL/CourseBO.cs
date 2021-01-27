using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public class CourseBO: DAL.CourseDBO
    {
        public IEnumerable<CourseSkillBO> Skills { get; set; }

        public IEnumerable<CourseMaterialBO> Materials { get; set; }
    }
}
