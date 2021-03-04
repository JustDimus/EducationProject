using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.Models
{
    public class Course: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public int? CreatorId { get; set; }

        public List<AccountCourse> AccountCourses { get; set; }

        public List<CourseMaterial> CourseMaterials { get; set; }

        public List<CourseSkill> CourseSkills { get; set; }

        public Account CreatorAccount { get; set; }
    }
}
