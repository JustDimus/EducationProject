using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL.EF
{
    public class CourseDBO: BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsVisible { get; set; }

        public int? CreatorId { get; set; }

        public List<AccountCourseDBO> AccountCourses { get; set; }

        public List<CourseMaterialDBO> CourseMaterials { get; set; }

        public List<CourseSkillDBO> CourseSkills { get; set; }

        public AccountDBO CreatorAccount { get; set; }
    }
}
