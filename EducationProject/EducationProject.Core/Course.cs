using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core
{
    public class Course: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Account Creator { get; set; }

        public IEnumerable<CourseSkill> Skills { get; set; }

        public IEnumerable<int> Materials { get; set; }
    }
}
