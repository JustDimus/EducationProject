using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.Core.DAL
{
    public class CourseMaterialDBO: BaseEntity
    {
        public int MaterialId { get; set; }

        public int CourseId { get; set; }

        public int Position { get; set; }
    }
}
