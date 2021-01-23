using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.BLL
{
    public class Course: DAL.Course
    {
        public IEnumerable<CourseSkill> Skills { get; set; }

        public IEnumerable<CourseMaterial> Materials { get; set; }
    }
}
